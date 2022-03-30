using Skeletonization.DataLayer.Data;
using System.Collections.Generic;

namespace Skeletonization.BusinessLayer.Abstractions
{
    public interface IVideoService
    {
        bool ReversePause();
        IEnumerable<VideoDeviceInfo> GetVideoDevices();
        void StartCamera(int cameraId, IVideoProcessingHandler handler);
        void StartFile(string filePath, IVideoProcessingHandler handler);
    }
}
