using Skeletonization.PresentationLayer.Detection.Models.Abstractions;

namespace Skeletonization.PresentationLayer.Detection.ViewModels
{
    internal class DetectionViewModel
    {
        public IDetectionModel Model { get; }

        public DetectionViewModel(IDetectionModel model)
        {
            Model = model;
        }
    }
}
