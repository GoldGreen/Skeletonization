using ReactiveUI;
using System.Collections.Generic;

namespace Skeletonization.PresentationLayer.Shared.Abstractions
{
    public interface IChecker<T, TRes> : IReactiveObject
    {
        IEnumerable<TRes> FailedCheckingElements { get; set; }
        void Check(IEnumerable<T> elements);
        bool Check(T el, out TRes res);
    }
}
