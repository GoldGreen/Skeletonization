using System.Drawing;
using System.Linq;

namespace Skeletonization.CrossLayer.Extensions
{
    public static class PointsExtensionsMethods
    {
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
