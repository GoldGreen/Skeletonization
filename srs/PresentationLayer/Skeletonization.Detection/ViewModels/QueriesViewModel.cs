using Prism.Events;
using ReactiveUI;
using Skeletonization.PresentationLayer.Detection.Models.Abstractions;
using Skeletonization.PresentationLayer.Shared.Data;
using Skeletonization.PresentationLayer.Shared.Extensions;
using Skeletonization.PresentationLayer.Shared.Prism;
using System;
using System.Windows.Input;

namespace Skeletonization.PresentationLayer.Detection.ViewModels
{
    internal class QueriesViewModel : ZonesConsumer
    {
        public IQueriesModel Model { get; }

        public ICommand AddQueryCommand { get; }
        public ICommand RemoveQueryCommand { get; }
        public ICommand AddZoneToQueryCommand { get; }
        public ICommand RemoveZoneFromQueryCommand { get; }

        public QueriesViewModel(IQueriesModel model, IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            Model = model;

            AddQueryCommand = ReactiveCommand.Create(() => Model.Queries.Add(new()));
            RemoveQueryCommand = ReactiveCommand.Create<Query>(x => Model.Queries.Remove(x));

            AddZoneToQueryCommand = ReactiveCommand.Create<(Zone, Query)>
            (
                x =>
                {
                    if (x.Item1 is null || x.Item2.QueriesZones.Contains(x.Item1))
                    {
                        EventAggregator.GetEvent<NotificationSended>()
                                       .Publish("Выбранная зона пустая или уже используется запросом");
                        return;
                    }

                    x.Item2.QueriesZones.Add(x.Item1);
                }
            );

            RemoveZoneFromQueryCommand = ReactiveCommand.Create<(Zone, Query)>(x => x.Item2.QueriesZones.Remove(x.Item1));
        }
    }
}
