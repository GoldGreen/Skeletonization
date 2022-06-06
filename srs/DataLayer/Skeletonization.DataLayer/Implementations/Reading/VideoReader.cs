using Emgu.CV;
using Microsoft.Extensions.Logging;
using Skeletonization.CrossfulLayer.Abstractions;
using Skeletonization.DataLayer.Abstractions;
using Skeletonization.DataLayer.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Skeletonization.DataLayer.Implementations.Reading
{
    internal class VideoReader : IVideoReader
    {
        private VideoCapture _videoCapture;
        public bool Paused
        {
            get
            {
                lock (this)
                {
                    return _paused;
                }
            }
            set
            {
                lock (this)
                {
                    _paused = value;
                }
            }
        }
        private bool _paused;

        public async void Start(IFactory<VideoCapture> videoCaptureFactory, Func<FrameInfo, Task> changingCallback, Action<VideoInfo> captureLoaded)
        {
            _videoCapture = videoCaptureFactory.Create();
            captureLoaded(new(_videoCapture.Width, _videoCapture.Height));

            await Task.Run(async () =>
            {
                int frameNum = 0;
                while (true)
                {
                    if (Paused)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    Mat frame = new();

                    try
                    {
                        if (!_videoCapture.Read(frame))
                        {
                            break;
                        }
                    }
                    catch (Exception)
                    {
                    }
                 

                    await changingCallback?.Invoke(new FrameInfo(frame, frameNum++));
                }
            });
            _videoCapture.Dispose();
        }
    }
}
