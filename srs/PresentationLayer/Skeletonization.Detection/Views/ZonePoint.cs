using Skeletonization.PresentationLayer.Shared.Reactive;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Skeletonization.PresentationLayer.Detection.Views
{
    public class ZonePoint : ZoneShapeBase
    {
        public Point Point
        {
            get => (Point)GetValue(PointProperty);
            set => SetValue(PointProperty, value);
        }

        public static readonly DependencyProperty PointProperty =
                   DependencyProperty.Register(nameof(Point),
                                               typeof(Point),
                                               typeof(ZonePoint),
                                               new(PointChangedCallback));

        private IDisposable _sub;
        private bool _isDragging;

        static ZonePoint()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZonePoint), new FrameworkPropertyMetadata(typeof(ZonePoint)));
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            var parent = Parent as Panel;

            var parentSizeChanged = parent.SizeChangedObservable()
                                          .Subscribe(_ => DrawPoint());

            var movingSub = parent.MouseMoveObservable()
                                  .Where(_ => _isDragging)
                                  .Select(x => x.GetPosition(parent))
                                  .Select(FromParent)
                                  .Subscribe(p => Point = p);

            var parentMouseUp = parent.MouseUpObservable()
                                      .Select(_ => false);

            var mouseDown = this.MouseDownObservable()
                                .Select(_ => true);

            var draggingSub = parentMouseUp.Merge(mouseDown)
                                           .Subscribe(x => _isDragging = x);

            _sub = new CompositeDisposable(parentSizeChanged, movingSub, draggingSub);
        }

        protected override void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _sub.Dispose();
        }

        private static void PointChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ZonePoint).DrawPoint();
        }

        private void DrawPoint()
        {
            var point = ToParent(Point);
            Canvas.SetLeft(this, point.X - Width / 2);
            Canvas.SetTop(this, point.Y - Height / 2);
        }
    }
}
