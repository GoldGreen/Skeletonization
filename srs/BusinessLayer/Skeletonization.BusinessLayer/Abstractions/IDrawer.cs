using Emgu.CV;
using System.Drawing;

namespace Skeletonization.BusinessLayer.Abstractions
{
    public interface IDrawer
    {
        void Draw(Mat mat, Point[,] points);
    }
}
