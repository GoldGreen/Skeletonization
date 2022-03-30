using DirectShowLib;
using Skeletonization.DataLayer.Abstractions;
using Skeletonization.DataLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Skeletonization.DataLayer.Implementations.Reading
{
    internal class VideoDevicesResolver : IVideoDevicesResolver
    {
        private readonly Lazy<IEnumerable<VideoDeviceInfo>> _videoDeviceInfos;

        public VideoDevicesResolver()
        {
            _videoDeviceInfos = new(() => LoadVideoDevicesInfo().ToList());
        }

        public IEnumerable<VideoDeviceInfo> ResolveVideoDevices()
        {
            return _videoDeviceInfos.Value;
        }

        private static IEnumerable<VideoDeviceInfo> LoadVideoDevicesInfo()
        {
            var captureDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            for (int idx = 0; idx < captureDevices.Length; idx++)
            {
                yield return new(idx, captureDevices[idx].Name);
            }
        }
    }
}
