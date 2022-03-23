using Prism.Ioc;
using Skeletonization.DataLayer.Reading.Abstractions;
using Skeletonization.DataLayer.Reading.Implementations;

namespace Skeletonization.DataLayer.Reading
{
    public static class LayerRegistration
    {
        public static void AddDataLayer(this IContainerRegistry container)
        {
            container.RegisterSingleton<IVideoReader, VideoReader>();
        }
    }
}
