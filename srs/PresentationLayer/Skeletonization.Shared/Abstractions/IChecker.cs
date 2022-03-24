using ReactiveUI;
using System.Collections.Generic;

namespace Skeletonization.PresentationLayer.Shared.Abstractions
{
    public interface IChecker<T> : IReactiveObject
    {
        IEnumerable<T> FailedCheckingElements { get; set; }
        void Check(IEnumerable<T> elements);
        bool Check(T el);
    }
}
