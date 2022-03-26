using Skeletonization.PresentationLayer.Shared.Data;
using System.Collections.Generic;

namespace Skeletonization.PresentationLayer.Detection.Models.Abstractions
{
    public interface IVideoDevicesResolver
    {
        IEnumerable<VideoDeviceInfo> ResolveVideoDevices();
    }
}
