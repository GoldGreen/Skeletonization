using ReactiveUI;
using Skeletonization.Zones.Models.Abstractions;

namespace Skeletonization.Zones.ViewModels
{
    internal class ZonesViewModel : ReactiveObject
    {
        public IZonesModel Model { get; }

        public ZonesViewModel(IZonesModel model)
        {
            Model = model;
        }
    }
}
