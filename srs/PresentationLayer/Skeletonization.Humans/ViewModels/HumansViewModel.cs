using Prism.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Skeletonization.PresentationLayer.Shared.Data;
using Skeletonization.PresentationLayer.Shared.Prism;
using System.Collections.Generic;

namespace Skeletonization.Humans.ViewModels
{
    internal class HumansViewModel : ReactiveObject
    {
        public IEventAggregator EventAggregator { get; }
        [Reactive] public IEnumerable<HumanWithRoi> Humans { get; set; }

        public HumansViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;

            EventAggregator.GetEvent<HumansChanged>()
                           .Subscribe(x => Humans = x);
        }
    }
}
