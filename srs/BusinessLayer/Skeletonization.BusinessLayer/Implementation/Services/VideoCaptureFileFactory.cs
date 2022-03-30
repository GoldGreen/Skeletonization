using Emgu.CV;
using Skeletonization.CrossfulLayer.Abstractions;

namespace Skeletonization.BusinessLayer.Implementation.Services
{
    public class VideoCaptureFileFactory : IFactory<VideoCapture>
    {
        public string FileName { get; }

        public VideoCaptureFileFactory(string fileName)
        {
            FileName = fileName;
        }

        public VideoCapture Create()
        {
            return new(FileName);
        }
    }
}
