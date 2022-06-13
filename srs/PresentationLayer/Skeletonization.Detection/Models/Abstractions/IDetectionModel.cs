using Emgu.CV;
using ReactiveUI;
using Skeletonization.BusinessLayer.Abstractions;
using System.Windows.Media;

namespace Skeletonization.PresentationLayer.Detection.Models.Abstractions
{
    public interface IDetectionModel : IReactiveObject, IVideoProcessingHandler
    {
        ImageSource FrameSource { get; set; }
        Mat Frame { get; set; }
        int FrameNum { get; set; }
        long FrameHandlingTime { get; set; }
        string VideoDescription { get; set; }
        bool Paused { get; set; }

        void StartVideoFromCamera(int cameraId);
        void StartVideoFromFile(string fileName);
    }
}
