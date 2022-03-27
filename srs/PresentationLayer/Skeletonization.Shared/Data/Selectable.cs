using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Skeletonization.PresentationLayer.Shared.Data
{
    public class Selectable<T> : ReactiveObject
    {
        public T Value { get; }
        [Reactive] public bool IsSelected { get; set; }

        public Selectable(T value)
        {
            Value = value;
        }
    }
}
