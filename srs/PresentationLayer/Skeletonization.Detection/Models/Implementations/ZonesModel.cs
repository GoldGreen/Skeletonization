using Emgu.CV;
using Emgu.CV.Structure;
using Prism.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Skeletonization.CrossLayer.Extensions;
using Skeletonization.PresentationLayer.Detection.Models.Abstractions;
using Skeletonization.PresentationLayer.Shared.Data;
using Skeletonization.PresentationLayer.Shared.Prism;
using Skeletonization.PresentationLayer.Shared.Reactive;
using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;

namespace Skeletonization.PresentationLayer.Detection.Models.Implementations
{
    internal class ZonesModel : ReactiveObject, IZonesModel
    {
        public IEventAggregator EventAggregator { get; }

        public ObservableCollection<Zone> Zones { get; } = new();
        [Reactive] public Zone SelectedZone { get; set; }

        [Reactive] private Mat Frame { get; set; }
        private ConcurrentDictionary<Zone, IDisposable> _frameRoiSubs = new();

        public ZonesModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;

            Zones.ToObservable()
                 .Subscribe(EventAggregator.GetEvent<ZonesChanged>().Publish)
                 .Cashe();

            Zones.ToObservable(NotifyCollectionChangedAction.Add)
                 .Subscribe(AddingZoneHandler)
                 .Cashe();

            Zones.ToObservable(NotifyCollectionChangedAction.Remove)
                 .Do(RemovingZonehandler)
                 .Where(x => x == SelectedZone)
                 .Subscribe(_ => SelectedZone = null)
                 .Cashe();

            EventAggregator.GetEvent<ZoneSelected>()
                           .ToObservable()
                           .Subscribe(zone => SelectedZone = zone)
                           .Cashe();

            EventAggregator.GetEvent<FrameChanged>()
                           .ToObservable()
                           .Do(_ => Frame = null)
                           .Subscribe(frame => Frame = frame)
                           .Cashe();
        }

        public void AddingZoneHandler(Zone zone)
        {
            var frameChanged = this.WhenAnyValue(x => x.Frame);

            var zonePoitnsChanged = zone.WhenAnyValue(x => x.Points)
                                        .Throttle(TimeSpan.FromMilliseconds(100))
                                        .ObserveOnDispatcher()
                                        .Select(_ => Frame);

            var sub = frameChanged.Merge(zonePoitnsChanged)
                                  .WhereNotNull()
                                  .Subscribe(frame =>
                                  {
                                      Mat roi = new(frame, CreateRect(zone, frame.Width, frame.Height));
                                      zone.FrameRoiBytes = roi.ToBytes();
                                  });

            _frameRoiSubs.TryAdd(zone, sub);
        }

        public void RemovingZonehandler(Zone zone)
        {
            if (_frameRoiSubs.TryRemove(zone, out var sub))
            {
                sub.Dispose();
            }
        }

        private static Rectangle CreateRect(Zone zone, int frameWidth, int frameHeight)
        {
            var points = zone.GetPoints();

            var (startX, startY, endX, endY) = (points.Min(p => p.X), points.Min(p => p.Y), points.Max(p => p.X), points.Max(p => p.Y));
            int x = (int)(frameWidth * startX);
            int y = (int)(frameHeight * startY);

            int width = (int)(frameWidth * endX) - x;
            int height = (int)(frameHeight * endY) - y;

            return new Rectangle(x, y, width, height);
        }
    }
}
