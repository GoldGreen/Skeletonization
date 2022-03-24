using Emgu.CV;
using Skeletonization.CrossfulLayer.Data;
using Skeletonization.DataLayer.Abstractions;
using Skeletonization.DataLayer.Reading.Abstractions;
using System;
using System.Threading.Tasks;

namespace Skeletonization.DataLayer.Reading.Implementations.Reading
{
    internal class VideoReader : IVideoReader
    {
        private IVideoCaptureFabric _videoCaptureFabric;
        private VideoCapture _videoCapture;

        public void SetVideoCaptureFabric(IVideoCaptureFabric videoCaptureFabric)
        {
            _videoCaptureFabric = videoCaptureFabric;
        }

        public async void Start(Func<Mat, Task> changingCallback, Action<Size> captureLoaded)
        {
            _videoCapture = _videoCaptureFabric.Create();
            captureLoaded(new(_videoCapture.Width, _videoCapture.Height));

            await Task.Run(async () =>
            {
                while (true)
                {
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
