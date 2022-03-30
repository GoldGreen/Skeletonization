using ReactiveUI;
using Skeletonization.CrossfulLayer.Abstractions;
using Skeletonization.PresentationLayer.Detection.Models.Abstractions;
using Skeletonization.PresentationLayer.Shared.Data;
using System.Reactive.Linq;
using System.Windows.Input;

namespace Skeletonization.PresentationLayer.Detection.ViewModels
{
    internal class ZonesViewModel : ReactiveObject
    {
        public IZonesModel Model { get; }
        public IFactory<Zone> ZoneFactory { get; }

        public ICommand AddZoneCommand { get; }
        public ICommand RemoveZoneCommand { get; }

        public ZonesViewModel(IZonesModel model, IFactory<Zone> zoneFactory)
        {
            Model = model;
            ZoneFactory = zoneFactory;

            AddZoneCommand = ReactiveCommand.Create(() =>
            {
                var zone = ZoneFactory.Create();
                Model.Zones.Add(zone);
                Model.SelectedZone = zone;
            });

            RemoveZoneCommand = ReactiveCommand.Create
            (
                () => Model.Zones.Remove(Model.SelectedZone),
                Model.WhenAnyValue(x => x.SelectedZone)
                     .Select(x => x is not null)
            );
        }
    }
}
