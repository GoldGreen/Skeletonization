using Skeletonization.PresentationLayer.Shared.Data;
using System;
using System.Globalization;

namespace Skeletonization.PresentationLayer.Shared.Converters
{
    public class QueryZoneConverter : MultiConverterBase<QueryZoneConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Zone)values[0], (Query)values[1]);
        }
    }
}
