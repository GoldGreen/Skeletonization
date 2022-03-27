﻿using Emgu.CV;
using Prism.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Skeletonization.CrossLayer.Data;
using Skeletonization.CrossLayer.Extensions;
using Skeletonization.PresentationLayer.Detection.Models.Abstractions;
using Skeletonization.PresentationLayer.Shared.Data;
using Skeletonization.PresentationLayer.Shared.Prism;
using Skeletonization.PresentationLayer.Shared.Reactive;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Skeletonization.PresentationLayer.Detection.Models.Implementations
{
    internal class ZonesModel : ReactiveObject, IZonesModel
    {
        public IEventAggregator EventAggregator { get; }

        public ObservableCollection<Zone> Zones { get; } = new();
        [Reactive] public Zone SelectedZone { get; set; }

        [Reactive] public IEnumerable<Human> Humans { get; set; }
        private IEnumerable<Human> _humansCashe;

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

            EventAggregator.GetEvent<HumansChanged>()
                           .ToObservable()
                           .WhereNotNull()
                           .Subscribe(x => Humans = x)
                           .Cashe();
        }

        public void AddingZoneHandler(Zone zone)
        {
            var frameChanged = this.WhenAnyValue(x => x.Frame);

            var zonePointsFrameChanged = zone.WhenAnyValue(x => x.Points)
                                        .Throttle(TimeSpan.FromMilliseconds(100))
                                        .ObserveOnDispatcher()
                                        .Select(_ => Frame);

            var sub = frameChanged.Merge(zonePointsFrameChanged)
                                  .WhereNotNull()
                                  .Subscribe(frame =>
                                  {
                                      Mat roi = new(frame, CreateRect(zone, frame.Width, frame.Height));
                                      zone.FrameRoiBytes = roi.ToBytes();
                                  });

            var zoneParametersChanged = zone.WhenAnyValue(x => x.Name, x => x.MinCount, x => x.Delay, x => x.CheckInside)
                                            .Select(_ => _humansCashe);

            var zonePointsHumanChanged = zone.WhenAnyValue(x => x.Points)
                                             .Throttle(TimeSpan.FromMilliseconds(100))
                                             .ObserveOnDispatcher()
                                             .Select(_ => _humansCashe);

            var humansChangedWithZoneParameters = zoneParametersChanged.Merge(zonePointsHumanChanged);
            foreach (var selectable in zone.BodyParts)
            {
                var selectChanged = selectable.WhenAnyValue(x => x.IsSelected)
                                              .Select(_ => _humansCashe);

                humansChangedWithZoneParameters = humansChangedWithZoneParameters.Merge(selectChanged);
            }

            var humansChanged = this.WhenAnyValue(x => x.Humans);
            humansChangedWithZoneParameters = humansChangedWithZoneParameters.Merge(humansChanged)
                                                                             .Throttle(TimeSpan.FromMilliseconds(1))
                                                                             .ObserveOnDispatcher()
                                                                             .WhereNotNull();

            var humansSub = humansChangedWithZoneParameters.Subscribe(zone.Check);
            _frameRoiSubs.TryAdd(zone, new CompositeDisposable(sub, humansSub));
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