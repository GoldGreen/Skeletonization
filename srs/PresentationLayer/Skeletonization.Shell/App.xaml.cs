using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using Skeletonization.DataLayer.Reading;
using Skeletonization.PresentationLayer.Detection;
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
            containerRegistry.AddDataLayer();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<DetectionModule>();
            base.ConfigureModuleCatalog(moduleCatalog);
        }
    }
}
