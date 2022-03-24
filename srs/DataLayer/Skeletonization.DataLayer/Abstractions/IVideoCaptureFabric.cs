using Emgu.CV;

namespace Skeletonization.DataLayer.Abstractions
{
    public interface IVideoCaptureFabric
    {
        VideoCapture Create();
    }
}
