using Skeletonization.PresentationLayer.Detection.Models.Abstractions;
using Skeletonization.PresentationLayer.Shared.Data;

namespace Skeletonization.PresentationLayer.Detection.Models.Implementations
{
    internal class ZoneFactory : IZoneFactory
    {
        public Zone Create()
        {
            return new(0.4, 0.4, 0.1, 0.1);
        }
    }
}
