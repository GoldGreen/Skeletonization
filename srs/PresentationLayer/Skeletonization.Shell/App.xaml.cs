using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Unity;
using Skeletonization.BusinessLayer;
using Skeletonization.DataLayer;
using Skeletonization.Humans;
using Skeletonization.PresentationLayer.Detection;
using Skeletonization.Settings;
using Skeletonization.Zones;
using System.Windows;

namespace Skeletonization.PresentationLayer.Shell
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<ShellWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ViewModelLocationProvider.Register<ShellWindow, ShellViewModel>();

            containerRegistry.AddBusinesLayer(); 
            containerRegistry.AddDataLayer();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<DetectionModule>();
            moduleCatalog.AddModule<ZonesModule>();
            moduleCatalog.AddModule<HumansModule>();
            moduleCatalog.AddModule<SettingsModule>();
            base.ConfigureModuleCatalog(moduleCatalog);
        }
    }
}
