using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Skeletonization.Constructor.Models.Abstractions;
using Skeletonization.Constructor.Models.Implementation;
using Skeletonization.Constructor.ViewModels;
using Skeletonization.Constructor.Views;
using Skeletonization.PresentationLayer.Shared.Prism;

namespace Skeletonization.Constructor
{
    public class ConstructorModule : ModuleBase
    {
        protected override void RegisterViews(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion<ConstructorControl>(GlobalRegions.Constructor);
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IConstructorModel, ConstructorModel>();

            ViewModelLocationProvider.Register<ConstructorControl, ConstructorViewModel>();
        }
    }
}
