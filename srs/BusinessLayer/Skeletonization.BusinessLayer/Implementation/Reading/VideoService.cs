using Emgu.CV;
using Skeletonization.BusinessLayer.Abstractions;
using Skeletonization.CrossfulLayer.Abstractions;
using Skeletonization.CrossLayer.Data;
using Skeletonization.CrossLayer.Exceptions;
using Skeletonization.DataLayer.Abstractions;
using Skeletonization.DataLayer.Implementations.Reading;
using System.Collections.Generic;

namespace Skeletonization.BusinessLayer.Implementation.Reading
{
    internal class VideoService : IVideoService
    {
        public IVideoReader VideoReader { get; }
        public IVideoDevicesResolver VideoDevicesResolver { get; }

        public VideoService(IVideoReader videoReader, IVideoDevicesResolver videoDevicesResolver)
        {
            VideoReader = videoReader;
            VideoDevicesResolver = videoDevicesResolver;
        }

        public bool ReversePause()
        {
            VideoReader.Paused = !VideoReader.Paused;
            return VideoReader.Paused;
        }

        private void Start(VideoCaptureFabricType fabricType, object arg, IVideoProcessingHandler handler)
        {
            IFactory<VideoCapture> fabric = fabricType switch
            {
                VideoCaptureFabricType.File => arg switch
                {
                    string filename => new VideoCaptureFileFactory(filename),
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

        public IEnumerable<VideoDeviceInfo> GetVideoDevices()
        {
            return VideoDevicesResolver.ResolveVideoDevices();
        }
    }
}
