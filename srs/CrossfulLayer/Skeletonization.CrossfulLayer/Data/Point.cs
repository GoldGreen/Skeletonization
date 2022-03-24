namespace Skeletonization.CrossfulLayer.Data
{
    public record Point(double X, double Y)
    {
        public static Point operator +(Point first, Point second)
        {
            return new Point(first.X + second.X, first.Y + second.Y);
        }

        public static Point operator -(Point first, Point second)
        {
            return new Point(first.X - second.X, first.Y - second.Y);
        }
    }
}
