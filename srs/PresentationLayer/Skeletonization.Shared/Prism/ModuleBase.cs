using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Skeletonization.PresentationLayer.Shared.Prism
{
    public abstract class ModuleBase : IModule
    {
        protected virtual void RegisterViews(IRegionManager regionManager)
        {

        }

        public virtual void OnInitialized(IContainerProvider containerProvider)
        {
            RegisterViews(containerProvider.Resolve<IRegionManager>());
        }

        public abstract void RegisterTypes(IContainerRegistry containerRegistry);
    }
}
