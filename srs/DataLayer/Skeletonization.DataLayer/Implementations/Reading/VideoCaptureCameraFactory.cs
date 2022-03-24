using Emgu.CV;
using Skeletonization.DataLayer.Abstractions;

namespace Skeletonization.DataLayer.Reading.Implementations.Reading
{
    public class VideoCaptureCameraFactory : IVideoCaptureFabric
    {
        public int CameraId { get; }

        public VideoCaptureCameraFactory(int cameraId)
        {
            CameraId = cameraId;
        }

        public VideoCapture Create()
        {
            return new(CameraId);
        }
    }
}
