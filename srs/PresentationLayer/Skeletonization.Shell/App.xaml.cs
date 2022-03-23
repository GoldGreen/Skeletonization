using Prism.Ioc;
using Prism.Unity;
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
    }
}
