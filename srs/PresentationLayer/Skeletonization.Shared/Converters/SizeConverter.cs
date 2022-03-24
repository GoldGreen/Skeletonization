using System;
using System.Globalization;
using System.Windows;
using D = Skeletonization.CrossLayer.Data;

namespace Skeletonization.PresentationLayer.Shared.Converters
{
    public class SizeConverter : ConverterBase<SizeConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not D.Size s)
            {
                return null;
            }

            return new Size(s.Width, s.Height);
        }
    }
}
