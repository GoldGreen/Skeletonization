using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
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
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<DetectionModule>();
            base.ConfigureModuleCatalog(moduleCatalog);
        }
    }
}
