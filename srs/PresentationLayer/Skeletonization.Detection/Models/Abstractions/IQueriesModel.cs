using ReactiveUI;
using Skeletonization.PresentationLayer.Shared.Data;
using System.Collections.ObjectModel;

namespace Skeletonization.PresentationLayer.Detection.Models.Abstractions
{
    public interface IQueriesModel : IReactiveObject
    {
        ObservableCollection<Query> Queries { get; }
    }
}
