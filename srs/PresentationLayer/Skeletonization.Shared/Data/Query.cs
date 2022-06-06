using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Skeletonization.PresentationLayer.Shared.Reactive;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System;
using System.Reactive;

namespace Skeletonization.PresentationLayer.Shared.Data
{
    public class Query : ReactiveObject
    {
        [Reactive] public bool IsInverted { get; set; }
        [Reactive] public bool IsDangerours { get; set; }

        [Reactive] public bool CheckInZone { get; set; }
        [Reactive] public Zone CheckingZone { get; set; }

        [Reactive] public bool SendReport { get; set; }

        public ObservableCollection<Zone> Zones { get; } = new();

        public Query()
        {
            this.WhenAnyValue(x => x.IsInverted, x => x.CheckInZone, x => x.CheckingZone)
                .Select(_ => Unit.Default)
                .Merge(Zones.ToObservable().Select(_ => Unit.Default))
                .Subscribe(_ => SendReport = false);
        }
    }
}