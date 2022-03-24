using Emgu.CV;
using Skeletonization.CrossLayer.Data;
using Skeletonization.DataLayer.Abstractions;
using Skeletonization.DataLayer.Reading.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Skeletonization.DataLayer.Reading.Implementations.Reading
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

        public async void Start(IVideoCaptureFabric videoCaptureFabric, Func<Mat, Task> changingCallback, Action<Size> captureLoaded)
        {
            _videoCapture = videoCaptureFabric.Create();
            captureLoaded(new(_videoCapture.Width, _videoCapture.Height));

            await Task.Run(async () =>
            {
                while (true)
                {
                    if (Paused)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    using Mat frame = new();

                    if (!_videoCapture.Read(frame))
                    {
                        break;
                    }

                    await changingCallback(frame);
                }
            });
            _videoCapture.Dispose();
        }
    }
}
