﻿using Emgu.CV;
using Skeletonization.CrossfulLayer.Data;
using Skeletonization.DataLayer.Abstractions;
using System;
using System.Threading.Tasks;

namespace Skeletonization.DataLayer.Reading.Abstractions
{
    public interface IVideoReader
    {
        void SetVideoCaptureFabric(IVideoCaptureFabric videoCaptureFabric);
        void Start(Func<Mat, Task> changingCallback, Action<Size> captureLoaded);
    }
}
