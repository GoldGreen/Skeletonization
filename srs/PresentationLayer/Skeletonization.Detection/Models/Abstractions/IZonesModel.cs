using ReactiveUI;
using Skeletonization.PresentationLayer.Shared.Data;
using System.Collections.ObjectModel;

namespace Skeletonization.PresentationLayer.Detection.Models.Abstractions
{
    public interface IZonesModel : IReactiveObject
    {
        ObservableCollection<Zone> Zones { get; }
        Zone SelectedZone { get; set; }
    }
}
