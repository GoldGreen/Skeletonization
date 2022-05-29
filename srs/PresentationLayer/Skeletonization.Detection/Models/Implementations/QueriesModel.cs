using Prism.Events;
using ReactiveUI;
using Skeletonization.PresentationLayer.Detection.Models.Abstractions;
using Skeletonization.PresentationLayer.Shared.Data;
using System.Collections.ObjectModel;

namespace Skeletonization.PresentationLayer.Detection.Models.Implementations
{
    internal class QueriesModel : ReactiveObject, IQueriesModel
    {
        public ObservableCollection<Query> Queries { get; } = new();
    }
}
