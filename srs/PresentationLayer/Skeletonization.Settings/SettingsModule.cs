using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Skeletonization.PresentationLayer.Shared.Prism;
using Skeletonization.Settings.Models.Abstractions;
using Skeletonization.Settings.Models.Implementation;
using Skeletonization.Settings.ViewModels;
using Skeletonization.Settings.Views;

namespace Skeletonization.Settings
{
    public class SettingsModule : ModuleBase
    {
        protected override void RegisterViews(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion<SettingsControl>(GlobalRegions.Settings);
        }

        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ISettingsModel, SettingsModel>();

            ViewModelLocationProvider.Register<SettingsControl, SettingsViewModel>();
        }
    }
}
