using Emgu.CV;
using System.Drawing;

namespace Skeletonization.BusinessLayer.Abstractions
{
    public interface IDetector
    {
        Point[,] Detect(Mat input);
    }
}
