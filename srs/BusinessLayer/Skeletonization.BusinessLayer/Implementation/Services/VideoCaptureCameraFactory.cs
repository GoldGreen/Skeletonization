using Emgu.CV;
using Skeletonization.CrossfulLayer.Abstractions;

namespace Skeletonization.BusinessLayer.Implementation.Services
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
