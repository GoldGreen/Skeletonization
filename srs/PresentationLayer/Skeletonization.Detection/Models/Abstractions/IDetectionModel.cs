using Emgu.CV;
using ReactiveUI;
using Skeletonization.BusinessLayer.Abstractions;

namespace Skeletonization.PresentationLayer.Detection.Models.Abstractions
{
    public interface IDetectionModel : IReactiveObject, IVideoProcessingHandler
    {
        byte[] FrameBytes { get; set; }
        Mat Frame { get; set; }
        int FrameNum { get; set; }
        long FrameHandlingTime { get; set; }
        string VideoDescription { get; set; }

        void StartVideoFromCamera(int cameraId);
        void StartVideoFromFile(string fileName);
    }
}
