using Emgu.CV;
using Skeletonization.CrossfulLayer.Abstractions;
using Skeletonization.DataLayer.Data;
using System;
using System.Threading.Tasks;

namespace Skeletonization.DataLayer.Abstractions
{
    public interface IVideoReader
    {
        bool Paused { get; set; }
        void Start(IFactory<VideoCapture> videoCaptureFactory, Func<FrameInfo, Task> changingCallback, Action<VideoInfo> captureLoaded);
    }
}
