using ReactiveUI;

namespace Skeletonization.PresentationLayer.Detection.Models.Abstractions
{
    public interface IDetectionModel : IReactiveObject
    {
        byte[] Frame { get; set; }
    }
}
