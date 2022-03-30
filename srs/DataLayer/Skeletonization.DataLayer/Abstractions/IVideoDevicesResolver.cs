using Skeletonization.DataLayer.Data;
using System.Collections.Generic;

namespace Skeletonization.DataLayer.Abstractions
{
    public interface IVideoDevicesResolver
    {
        IEnumerable<VideoDeviceInfo> ResolveVideoDevices();
    }
}
