using Skeletonization.CrossLayer.Data;
using System.Collections.Generic;
using System.Windows;

namespace Skeletonization.PresentationLayer.Shared.Views
{
    public class HumanResultControl : HumanControl
    {
        public IEnumerable<BodyPart> BodyParts
        {
            get => (IEnumerable<BodyPart>)GetValue(BodyPartsProperty);
            set => SetValue(BodyPartsProperty, value);
        }

        public static readonly DependencyProperty BodyPartsProperty =
           DependencyProperty.Register(nameof(BodyParts), typeof(IEnumerable<BodyPart>), typeof(HumanControl));

        static HumanResultControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HumanResultControl), new FrameworkPropertyMetadata(typeof(HumanResultControl)));
        }
    }
}
