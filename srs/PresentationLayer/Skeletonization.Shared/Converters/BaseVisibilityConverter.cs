using System;
using System.Globalization;
using System.Windows;

namespace Skeletonization.PresentationLayer.Shared.Converters
{
    public abstract class BaseVisibilityConverter<T, TSelf> : ConverterBase<TSelf>
        where TSelf : BaseVisibilityConverter<T, TSelf>, new()
    {
        public abstract T VisibleValue { get; }
        public abstract T HiddenValue { get; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isReversed = "reversed".Equals(parameter);

            if (value.Equals(VisibleValue) ^ isReversed)
            {
                return Visibility.Visible;
            }

            if (value.Equals(HiddenValue) ^ isReversed)
            {
                return Visibility.Hidden;
            }

            return null;
        }
    }
}
