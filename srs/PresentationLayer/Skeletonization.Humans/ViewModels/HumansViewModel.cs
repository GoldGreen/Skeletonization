using ReactiveUI;
using Skeletonization.Humans.Models.Abstractions;

namespace Skeletonization.Humans.ViewModels
{
    internal class HumansViewModel : ReactiveObject
    {
        public IHumansModel Model { get; }

        public HumansViewModel(IHumansModel model)
        {
            Model = model;
        }
    }
}
