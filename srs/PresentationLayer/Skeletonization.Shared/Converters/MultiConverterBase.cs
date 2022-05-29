using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Skeletonization.PresentationLayer.Shared.Converters
{
    public abstract class MultiConverterBase<T> : MarkupExtension, IMultiValueConverter
        where T : MultiConverterBase<T>, new()
    {
        private T _converter;

        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ??= new();
        }
    }
}
