using System;
using System.ComponentModel;

namespace Skeletonization.PresentationLayer.Shared.Extensions
{
    public static class EnumExtensionsMethods
    {
        public static string ToDescriptionOrString(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}
