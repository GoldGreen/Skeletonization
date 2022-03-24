using Emgu.CV;
using Skeletonization.CrossLayer.Data;
using Skeletonization.DataLayer.Abstractions;
using System;
using System.Threading.Tasks;

namespace Skeletonization.DataLayer.Reading.Abstractions
{
    public interface IVideoReader
    {
        bool Paused { get; set; }

        void Start(IVideoCaptureFabric videoCaptureFabric, Func<Mat, Task> changingCallback, Action<Size> captureLoaded);
    }
}
