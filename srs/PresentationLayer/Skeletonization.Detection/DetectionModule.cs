using Prism.Ioc;
using Prism.Regions;
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
        }
    }
}
