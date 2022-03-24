using Emgu.CV;
using Skeletonization.BusinessLayer.Abstractions;
using Skeletonization.CrossfulLayer.Data;
using Skeletonization.CrossfulLayer.Exceptions;
using Skeletonization.DataLayer.Abstractions;
using Skeletonization.DataLayer.Implementations.Reading;
using Skeletonization.DataLayer.Reading.Abstractions;
using System;
using System.Threading.Tasks;

namespace Skeletonization.BusinessLayer.Implementation.Reading
{
    internal class VideoService : IVideoService
    {
        public IVideoReader VideoReader { get; }

        public VideoService(IVideoReader videoReader)
        {
            VideoReader = videoReader;
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

            VideoReader.SetVideoCaptureFabric(fabric);
            VideoReader.Start(handler.HandleFrame, handler.HandleVideoInformation);
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
