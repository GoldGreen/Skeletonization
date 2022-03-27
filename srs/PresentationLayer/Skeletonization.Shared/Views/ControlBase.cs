using System.Windows;
using System.Windows.Controls;

namespace Skeletonization.PresentationLayer.Shared.Views
{
    public class ControlBase : Control
    {
        static ControlBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ControlBase), new FrameworkPropertyMetadata(typeof(ControlBase)));
        }

        public ControlBase()
        {
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        protected virtual void OnUnloaded(object sender, RoutedEventArgs e)
        {
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
