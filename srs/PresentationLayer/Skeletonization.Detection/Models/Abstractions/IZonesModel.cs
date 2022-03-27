using ReactiveUI;
using Skeletonization.CrossLayer.Data;
using Skeletonization.PresentationLayer.Shared.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Skeletonization.PresentationLayer.Detection.Models.Abstractions
{
    public interface IZonesModel : IReactiveObject
    {
        ObservableCollection<Zone> Zones { get; }
        Zone SelectedZone { get; set; }
        IEnumerable<Human> Humans { get; set; }
    }
}
