using Skeletonization.CrossfulLayer.Data;
using Skeletonization.CrossLayer.Data;
using Skeletonization.DataLayer.Abstractions;
using System;
using System.Threading.Tasks;

namespace Skeletonization.DataLayer.Reading.Abstractions
{
    public interface IVideoReader
    {
        bool Paused { get; set; }
        void Start(IVideoCaptureFabric videoCaptureFabric, Func<FrameInfo, Task> changingCallback, Action<VideoInfo> captureLoaded);
    }
}
