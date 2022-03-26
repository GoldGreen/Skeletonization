﻿using ReactiveUI;
using Skeletonization.PresentationLayer.Shared.Converters;
using Skeletonization.PresentationLayer.Shared.Data;
using Skeletonization.PresentationLayer.Shared.Reactive;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using D = Skeletonization.CrossLayer.Data;

namespace Skeletonization.PresentationLayer.Detection.Views
{
    public class ZonePolygon : ZoneShapeBase
    {
        public Zone Zone
        {
            get => (Zone)GetValue(ZoneProperty);
            set => SetValue(ZoneProperty, value);
        }

        public PointCollection Points
        {
            get => (PointCollection)GetValue(PointsProperty);
            private set => SetValue(PointsProperty, value);
        }

        public static readonly DependencyProperty ZoneProperty =
           DependencyProperty.Register(nameof(Zone),
                                       typeof(Zone),
                                       typeof(ZonePolygon));

        public static readonly DependencyProperty PointsProperty =
          DependencyProperty.Register(nameof(Points),
                                      typeof(PointCollection),
                                      typeof(ZonePolygon));

        private IDisposable _movingSub;
        private IDisposable _draggingSub;
        private IDisposable _pointsSub;

        private bool _isDragging;
        private Point _lastPosition;

        static ZonePolygon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZonePolygon),
                new FrameworkPropertyMetadata(typeof(ZonePolygon)));
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            var resizing = (Parent as Panel).SizeChangedObservable()
                                              .Select(_ => Zone.Points);

            _pointsSub = Zone.WhenAnyValue(x => x.Points)
                                 .Merge(resizing)
                                 .Select(x => x.Select(x => new Point(x.X, x.Y)))
                                 .Select(x => x.Select(ToParent))
                                 .Select(x => new PointCollection(x))
                                 .Subscribe(x => Points = x);

            var parent = Parent as Panel;
            _movingSub = parent.MouseMoveObservable()
                               .Where(_ => _isDragging)
                               .Select(x =>
                               {
                                   var lastPosition = _lastPosition;
                                   _lastPosition = x.GetPosition(parent);
                                   return _lastPosition - lastPosition;
                               })
                               .Select(v => new Point(v.X, v.Y))
                               .Select(FromParent)
                               .Select(v => new D.Point(v.X, v.Y))
                               .Subscribe(v =>
                               {
                                   Zone.LeftTop += v;
                                   Zone.RightTop += v;
                                   Zone.RightBot += v;
                                   Zone.LeftBot += v;
                               });

            var parentMouseUp = parent.MouseUpObservable()
                                      .Select(_ => false);

            var mouseDown = this.MouseDownObservable()
                                .Select(x => x.GetPosition(parent))
                                .Do(x => _lastPosition = x)
                                .Select(_ => true);

            _draggingSub = parentMouseUp.Merge(mouseDown)
                                        .Subscribe(x => _isDragging = x);
        }

        protected override void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _movingSub?.Dispose();
            _draggingSub?.Dispose();
            _pointsSub?.Dispose();
        }
    }
}
