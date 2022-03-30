using Emgu.CV;
using Skeletonization.CrossfulLayer.Abstractions;

namespace Skeletonization.DataLayer.Implementations.Reading
{
    public class VideoCaptureCameraFactory : IFactory<VideoCapture>
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
