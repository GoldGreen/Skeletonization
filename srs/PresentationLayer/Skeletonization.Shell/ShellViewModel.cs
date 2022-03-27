using MaterialDesignThemes.Wpf;
using Prism.Events;
using ReactiveUI;
using Skeletonization.PresentationLayer.Shared.Prism;
using Skeletonization.PresentationLayer.Shared.Reactive;
using System;
using System.Reactive.Linq;
using System.Windows;

namespace Skeletonization.PresentationLayer.Shell
{
    internal class ShellViewModel : ReactiveObject
    {
        public IEventAggregator EventAggregator { get; }
        public SnackbarMessageQueue Queue { get; } = new(TimeSpan.FromSeconds(2))
        {
            DiscardDuplicates = false
        };

        public ShellViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;

            EventAggregator.GetEvent<NotificationSended>()
                           .ToObservable()
                           .Subscribe(x => Queue.Enqueue(x, "OK", actionHandler: null))
                           .Cashe();
        }
    }
}
