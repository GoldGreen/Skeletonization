using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Skeletonization.Humans.Models.Abstractions;
using Skeletonization.Humans.Models.Implementation;
using Skeletonization.Humans.ViewModels;
using Skeletonization.Humans.Views;
using Skeletonization.PresentationLayer.Shared.Prism;

namespace Skeletonization.Humans
{
    public class HumansModule : ModuleBase
    {
        protected override void RegisterViews(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion<HumansControl>(GlobalRegions.Humans);
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IHumansModel, HumansModel>();

            ViewModelLocationProvider.Register<HumansControl, HumansViewModel>();
        }
    }
}
