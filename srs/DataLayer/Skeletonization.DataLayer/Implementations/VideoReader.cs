using Emgu.CV;
using Skeletonization.DataLayer.Reading.Abstractions;
using System;
using System.Threading.Tasks;

namespace Skeletonization.DataLayer.Reading.Implementations
{
    internal class VideoReader : IVideoReader
    {
        public async void Start(Func<Mat, Task> changingCallback)
        {
            using VideoCapture videoCapture = new(0);
            await Task.Run(async () =>
            {
                while (true)
                {
                    using Mat frame = new();
            
                    if (!videoCapture.Read(frame))
                    {
                        break;
                    }
            
                    await changingCallback(frame);
                }
            });
        }
    }
}
