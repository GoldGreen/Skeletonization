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
            regionManager.RegisterViewWithRegion<DetectionControl>(GlobalRegions.Detection)
                         .RegisterViewWithRegion<ZonesControl>(Regions.Zones);
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IDetectionModel, DetectionModel>();
            containerRegistry.RegisterSingleton<IZonesModel, ZonesModel>();

            containerRegistry.RegisterSingleton<IZoneFactory, ZoneFactory>();

            containerRegistry.RegisterDialog<OpenCameraDialogControl, OpenCameraDialogViewModel>("openCameraDialog");

            ViewModelLocationProvider.Register<DetectionControl, DetectionViewModel>();
            ViewModelLocationProvider.Register<ZonesControl, ZonesViewModel>();
        }
    }
}
