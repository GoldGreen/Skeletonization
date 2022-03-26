using Skeletonization.PresentationLayer.Shared.Reactive;
using System;
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

        private IDisposable _parentSizeChanged;
        private IDisposable _movingSub;
        private IDisposable _draggingSub;
        private bool _isDragging;

        static ZonePoint()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZonePoint), new FrameworkPropertyMetadata(typeof(ZonePoint)));
        }

        protected override void OnLoaded(object sender, RoutedEventArgs e)
        {
            var parent = Parent as Panel;

            _parentSizeChanged = parent.SizeChangedObservable()
                                       .Subscribe(_ => DrawPoint());

            _movingSub = parent.MouseMoveObservable()
                               .Where(_ => _isDragging)
                               .Select(x => x.GetPosition(parent))
                               .Select(FromParent)
                               .Subscribe(p => Point = p);

            var parentMouseUp = parent.MouseUpObservable()
                                      .Select(_ => false);

            var mouseDown = this.MouseDownObservable()
                                .Select(_ => true);

            _draggingSub = parentMouseUp.Merge(mouseDown)
                                        .Subscribe(x => _isDragging = x);
        }

        protected override void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _movingSub?.Dispose();
            _draggingSub?.Dispose();
            _parentSizeChanged?.Dispose();
        }

        private static void PointChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ZonePoint z)
            {
                return;
            }

            z.DrawPoint();
        }

        private void DrawPoint()
        {
            var point = ToParent(Point);
            Canvas.SetLeft(this, point.X - Width / 2);
            Canvas.SetTop(this, point.Y - Height / 2);
        }
    }
}
