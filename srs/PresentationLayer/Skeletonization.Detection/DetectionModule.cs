using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Skeletonization.PresentationLayer.Detection.Models.Abstractions;
using Skeletonization.PresentationLayer.Detection.Models.Implementations;
using Skeletonization.PresentationLayer.Detection.ViewModels;
using Skeletonization.PresentationLayer.Detection.Views;
using Skeletonization.PresentationLayer.Shared.Prism;

namespace Skeletonization.PresentationLayer.Detection
{
    public class DetectionModule : ModuleBase
    {
        protected override void RegisterViews(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion<DetectionControl>(GlobalRegions.Detection);
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IDetectionModel, DetectionModel>();

            ViewModelLocationProvider.Register<DetectionControl, DetectionViewModel>();
        }
    }
}
