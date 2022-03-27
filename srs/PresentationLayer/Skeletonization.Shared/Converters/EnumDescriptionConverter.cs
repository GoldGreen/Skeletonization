using Skeletonization.PresentationLayer.Shared.Extensions;
using System;
using System.Globalization;

namespace Skeletonization.PresentationLayer.Shared.Converters
{
    public class EnumDescriptionConverter : ConverterBase<EnumDescriptionConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as Enum)?.ToDescriptionOrString();
        }
    }
}
