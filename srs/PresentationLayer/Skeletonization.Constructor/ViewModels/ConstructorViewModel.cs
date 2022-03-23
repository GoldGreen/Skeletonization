using ReactiveUI;
using Skeletonization.Constructor.Models.Abstractions;

namespace Skeletonization.Constructor.ViewModels
{
    internal class ConstructorViewModel : ReactiveObject
    {
        public IConstructorModel Model { get; }

        public ConstructorViewModel(IConstructorModel model)
        {
            Model = model;
        }
    }
}
