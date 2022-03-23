using Emgu.CV;
using Skeletonization.DataLayer.Reading.Abstractions;
using System;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace Skeletonization.DataLayer.Reading.Implementations
{
    internal class VideoReader : IVideoReader
    {
        public void Start(Func<Mat, Task> changingCallback)
        {
            _ = Task.Run(async () =>
                {
                    using VideoCapture videoCapture = new(0);
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
