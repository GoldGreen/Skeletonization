using System.Drawing;

namespace Skeletonization.BusinessLayer.Implementation.Detection
{
    internal struct KeyPoint
    {
        public KeyPoint(Point point, float probability)
        {
            Id = -1;
            Point = point;
            Probability = probability;
        }

        public int Id { get; set; }
        public Point Point { get; set; }
        public float Probability { get; set; }
    }
}
