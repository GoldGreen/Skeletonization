using Prism.Events;
using ReactiveUI;
using Skeletonization.PresentationLayer.Detection.Models.Abstractions;
using Skeletonization.PresentationLayer.Shared.Data;
using Skeletonization.PresentationLayer.Shared.Extensions;
using Skeletonization.PresentationLayer.Shared.Reactive;
using System.Collections.Specialized;
using System.Windows.Input;
using System;

namespace Skeletonization.PresentationLayer.Detection.ViewModels
{
    internal class QueriesViewModel : ZonesConsumer
    {
        public IQueriesModel Model { get; }

        public ICommand AddQueryCommand { get; }
        public ICommand RemoveQueryCommand { get; }

        public QueriesViewModel(IQueriesModel model, IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            Model = model;

            AddQueryCommand = ReactiveCommand.Create(() => Model.Queries.Add(new()));
            RemoveQueryCommand = ReactiveCommand.Create<Query>(x => Model.Queries.Remove(x));

            Zones.ToObservable(NotifyCollectionChangedAction.Remove)
                 .Subscribe(z =>
                 {
                     foreach (var query in Model.Queries)
                     {
                         query.Zones.Remove(z);
                     }
                 }).Cashe();
        }
    }
}
