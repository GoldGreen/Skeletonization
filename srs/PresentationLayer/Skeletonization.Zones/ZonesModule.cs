using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Skeletonization.PresentationLayer.Shared.Prism;
using Skeletonization.Zones.ViewModels;
using Skeletonization.Zones.Views;

namespace Skeletonization.Zones
{
    public class ZonesModule : ModuleBase
    {
        protected override void RegisterViews(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion<ZonesControl>(GlobalRegions.Zones);
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ViewModelLocationProvider.Register<ZonesControl, ZonesViewModel>();
        }
    }
}
