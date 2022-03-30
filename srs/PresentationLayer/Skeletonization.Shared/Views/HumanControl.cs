using Skeletonization.CrossLayer.Data;
using System.Windows;
using System.Windows.Controls;

namespace Skeletonization.PresentationLayer.Shared.Views
{
    public class HumanControl : Control
    {
        public Human Human
        {
            get => (Human)GetValue(HumanProperty);
            set => SetValue(HumanProperty, value);
        }

        public static readonly DependencyProperty HumanProperty =
            DependencyProperty.Register(nameof(Human), typeof(Human), typeof(HumanControl));

        static HumanControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HumanControl), new FrameworkPropertyMetadata(typeof(HumanControl)));
        }
    }
}
