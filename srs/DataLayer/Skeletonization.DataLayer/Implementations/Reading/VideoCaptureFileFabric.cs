using Emgu.CV;
using Skeletonization.DataLayer.Abstractions;

namespace Skeletonization.DataLayer.Implementations.Reading
{
    public class VideoCaptureFileFabric : IVideoCaptureFabric
    {
        public string FileName { get; }

        public VideoCaptureFileFabric(string fileName)
        {
            FileName = fileName;
        }

        public VideoCapture Create()
        {
            return new(FileName);
        }
    }
}
