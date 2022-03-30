using Skeletonization.BusinessLayer.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Skeletonization.PresentationLayer.Shared.Converters
{
    public class BodyPartsSelectorConverter : ConverterBase<BodyPartsSelectorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not IEnumerable<BodyPartPoint> bp)
            {
                return null;
            }

            return bp.Select(x => x.BodyPart).ToList();
        }
    }
}
