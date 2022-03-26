using System;
using System.Globalization;

namespace Skeletonization.PresentationLayer.Shared.Converters
{
    public class IsNullableConverter : ConverterBase<IsNullableConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }
    }
}
