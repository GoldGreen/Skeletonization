using Emgu.CV;
using System;
using System.Threading.Tasks;

namespace Skeletonization.DataLayer.Reading.Abstractions
{
    public interface IVideoReader
    {
        void Start(Func<Mat, Task> changingCallback);
    }
}
