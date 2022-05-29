using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;

namespace Skeletonization.PresentationLayer.Shared.Data
{
    public class Query : ReactiveObject
    {
        [Reactive] public bool IsInverted { get; set; }

        [Reactive] public bool CheckInZone { get; set; }
        [Reactive] public Zone CheckingZone { get; set; }

        public ObservableCollection<Zone> QueriesZones { get; } = new();
    }
}
