using System;
using System.Globalization;
using System.Windows;
using D = Skeletonization.CrossfulLayer.Data;

namespace Skeletonization.PresentationLayer.Shared.Converters
{
    public class PointConverter : ConverterBase<PointConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not D.Point p)
            {
                return null;
            }

            return new Point(p.X, p.Y);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not Point p)
            {
                return null;
            }

            return new D.Point(p.X, p.Y);
        }
    }
}
