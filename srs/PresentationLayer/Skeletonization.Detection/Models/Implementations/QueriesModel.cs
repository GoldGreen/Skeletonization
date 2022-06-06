using Emgu.CV;
using Prism.Events;
using ReactiveUI;
using Skeletonization.BusinessLayer.Abstractions;
using Skeletonization.BusinessLayer.Data;
using Skeletonization.PresentationLayer.Detection.Models.Abstractions;
using Skeletonization.PresentationLayer.Shared.Data;
using Skeletonization.PresentationLayer.Shared.Prism;
using Skeletonization.PresentationLayer.Shared.Reactive;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Skeletonization.PresentationLayer.Detection.Models.Implementations
{
    internal class QueriesModel : ReactiveObject, IQueriesModel
    {
        public ObservableCollection<Query> Queries { get; } = new();
        public IEventAggregator EventAggregator { get; }
        public IReportService ReportService { get; }
        public Mat Frame { get; set; }

        public QueriesModel(IEventAggregator eventAggregator, IReportService reportService)
        {
            EventAggregator = eventAggregator;
            ReportService = reportService;

            EventAggregator.GetEvent<FrameChanged>()
                           .ToObservable()
                           .Do(_ => Frame = null)
                           .Subscribe(frame => Frame = frame)
                           .Cashe();

            EventAggregator.GetEvent<HumansChanged>()
                           .ToObservable()
                           .Where(_ => Queries.Count > 0 && Frame != null)
                           .RecieveBlock(TimeSpan.FromSeconds(5))
                           .Select(x => x.Select(x => (Human)x))
                           .Select(async h =>
                           {
                               var tasks = Queries.Select(x => (query: x, humans: CheckHumans(x, h)))
                                                  .Where(x => x.query.SendReport && x.humans.Count > 0)
                                                  .Select(async x =>
                                                  {
                                                      using var frame = Frame.Clone();
                                                      await ReportService.SendReport(new("Нарушение!",
                                                                                         "Описание нарушения",
                                                                                         x.humans,
                                                                                         frame));
                                                  });

                               await Task.WhenAll(tasks);
                               return Unit.Default;
                           }).Concat().Subscribe().Cashe();
        }

        private static List<Human> CheckHumans(Query query, IEnumerable<Human> checkingHumans)
        {
            var queryHumans = query.CheckInZone switch
            {
                false => checkingHumans,
                true => query.CheckingZone?.FailedCheckingElements?.Select(x => x.Human).ToList() ?? Enumerable.Empty<Human>(),
            };

            List<Human> wrongHumans = new();

            foreach (var queryZone in query.Zones.GroupBy(x => x.Name))
            {
                var zoneHumans = queryZone.SelectMany(x => x.FailedCheckingElements?.Select(x => x.Human) ?? Enumerable.Empty<Human>())
                                          .ToList();

                foreach (var human in queryHumans)
                {
                    bool zoneHumansContains = zoneHumans.Any(x => x.Name == human.Name);

                    if (zoneHumansContains ^ query.IsInverted)
                    {
                        wrongHumans.Add(human);
                    }
                }
            }

            var result = wrongHumans.Distinct().OrderBy(x => x.Name).ToList();
            query.IsDangerours = result.Count > 0;
            return result;
        }
    }
}
