
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Skeletonization.PresentationLayer.Shared.Converters
{
    public abstract class ConverterBase<T> : MarkupExtension, IValueConverter
        where T : ConverterBase<T>, new()
    {
        private T _converter;

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ??= new();
        }
    }
}
