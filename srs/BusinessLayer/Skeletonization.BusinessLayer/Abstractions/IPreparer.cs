using System.Drawing;

namespace Skeletonization.BusinessLayer.Abstractions
{
    public interface IPreparer
    {
        Point[,] Prepare(Point[,] points);
    }
}
