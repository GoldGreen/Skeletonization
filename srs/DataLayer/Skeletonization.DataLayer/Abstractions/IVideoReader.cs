using Skeletonization.CrossfulLayer.Data;
using Skeletonization.CrossLayer.Data;
using System;
using System.Threading.Tasks;

namespace Skeletonization.DataLayer.Abstractions
{
    public interface IVideoReader
    {
        bool Paused { get; set; }
        void Start(IVideoCaptureFabric videoCaptureFabric, Func<FrameInfo, Task> changingCallback, Action<VideoInfo> captureLoaded);
    }
}
