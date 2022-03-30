using Prism.Events;
using ReactiveUI;
using Skeletonization.PresentationLayer.Shared.Extensions;

namespace Skeletonization.Zones.ViewModels
{
    internal class ZonesViewModel : ZonesConsumer, IReactiveObject
    {
        public ZonesViewModel(IEventAggregator eventAggregator) 
            : base(eventAggregator)
        {
        }
    }
}
