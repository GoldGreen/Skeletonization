using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Skeletonization.PresentationLayer.Detection.Views
{
    public class ZoneShapeBase : Control
    {
        public Brush Fill
        {
            get => (Brush)GetValue(FillProperty);
            set => SetValue(FillProperty, value);
        }

        public static readonly DependencyProperty FillProperty =
                DependencyProperty.Register(nameof(Fill),
                                            typeof(Brush),
                                            typeof(ZoneShapeBase));
        static ZoneShapeBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ZoneShapeBase), new FrameworkPropertyMetadata(typeof(ZoneShapeBase)));
        }

        protected Point ToParent(Point point)
        {
            var parent = Parent as Panel;
            return new Point(parent.ActualWidth * point.X, parent.ActualHeight * point.Y);
        }

        protected Point FromParent(Point point)
        {
            var parent = Parent as Panel;
            return new Point(point.X / parent.ActualWidth, point.Y / parent.ActualHeight);
        }
    }
}
