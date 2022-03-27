using Prism.Events;
using ReactiveUI;
using Skeletonization.PresentationLayer.Shared.Data;
using Skeletonization.PresentationLayer.Shared.Prism;
using Skeletonization.PresentationLayer.Shared.Reactive;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Linq;

namespace Skeletonization.PresentationLayer.Shared.ViewModels
{
    public class ZonesConsumerViewModel : ReactiveObject
    {
        public IEventAggregator EventAggregator { get; }
        public ObservableCollection<Zone> Zones { get; } = new();

        public ZonesConsumerViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;

            var zoneChanged = EventAggregator.GetEvent<ZonesChanged>()
                                             .ToObservable();

            zoneChanged.Where(x => x.action == NotifyCollectionChangedAction.Add)
                       .Select(x => x.zone)
                       .Subscribe(Zones.Add)
                       .Cashe();

            zoneChanged.Where(x => x.action == NotifyCollectionChangedAction.Remove)
                       .Select(x => x.zone)
                       .Subscribe(x => Zones.Remove(x))
                       .Cashe();
        }
    }
}
