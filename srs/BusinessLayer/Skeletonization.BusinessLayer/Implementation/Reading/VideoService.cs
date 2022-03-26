using Skeletonization.BusinessLayer.Abstractions;
using Skeletonization.CrossLayer.Data;
using Skeletonization.CrossLayer.Exceptions;
using Skeletonization.DataLayer.Abstractions;
using Skeletonization.DataLayer.Implementations.Reading;

namespace Skeletonization.BusinessLayer.Implementation.Reading
{
    internal class VideoService : IVideoService
    {
        public IVideoReader VideoReader { get; }

        public VideoService(IVideoReader videoReader)
        {
            VideoReader = videoReader;
        }

        public bool ReversePause()
        {
            VideoReader.Paused = !VideoReader.Paused;
            return VideoReader.Paused;
        }

        private void Start(VideoCaptureFabricType fabricType, object arg, IVideoProcessingHandler handler)
        {
            IVideoCaptureFabric fabric = fabricType switch
            {
                VideoCaptureFabricType.File => arg switch
                {
                    string filename => new VideoCaptureFileFabric(filename),
                    _ => throw new VideoCaptureFabricException(fabricType, arg, typeof(string))
                },
                VideoCaptureFabricType.Camera => arg switch
                {
                    int cameraId => new VideoCaptureCameraFactory(cameraId),
                    _ => throw new VideoCaptureFabricException(fabricType, arg, typeof(int))
                },
                _ => throw new VideoCaptureFabricException(fabricType)
            };

            VideoReader.Start(fabric, handler.HandleFrame, handler.HandleVideoInformation);
        }

        public void StartFile(string filePath, IVideoProcessingHandler handler)
        {
            Start(VideoCaptureFabricType.File, filePath, handler);
        }

        public void StartCamera(int cameraId, IVideoProcessingHandler handler)
        {
            Start(VideoCaptureFabricType.Camera, cameraId, handler);
        }
    }
}
