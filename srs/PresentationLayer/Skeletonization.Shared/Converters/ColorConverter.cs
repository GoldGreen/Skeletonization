using Skeletonization.PresentationLayer.Shared.Extensions;
using System;
using System.Globalization;
using System.Windows.Media;

namespace Skeletonization.PresentationLayer.Shared.Converters
{
    public class ColorConverter : ConverterBase<ColorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string str)
            {
                return null;
            }

            return str.ToBrush();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not SolidColorBrush color)
            {
                return null;
            }

            return color.ToName();
        }
    }
}
