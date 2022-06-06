using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace Skeletonization.CrossLayer.Extensions
{
    public static class PointsExtensionsMethods
    {
        public static string ToDescriptionOrString(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        public static bool IsEmpty(this Point point)
        {
            return point.X == -1 && point.Y == -1;
        }

        public static Point Average(params Point[] points)
        {
            var sum = new Point();
            int count = 0;

            foreach (var point in points.Where(x => !x.IsEmpty()))
            {
                sum.X += point.X;
                sum.Y += point.Y;
                count++;
            }

            if (count != 0)
            {
                sum.X /= count;
                sum.Y /= count;
            }
            else
            {
                return new Point(-1, -1);
            }

            return sum;
        }
    }
}
