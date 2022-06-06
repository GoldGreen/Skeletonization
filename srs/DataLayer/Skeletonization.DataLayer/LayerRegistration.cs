using Prism.Ioc;
using Skeletonization.DataLayer.Abstractions;
using Skeletonization.DataLayer.Implementations.DatabaseSending;
using Skeletonization.DataLayer.Implementations.Reading;
using Skeletonization.DataLayer.Implementations.TeamsSending;

namespace Skeletonization.DataLayer
{
    public static class LayerRegistration
    {
        public static void AddDataLayer(this IContainerRegistry container)
        {
            container.RegisterSingleton<IVideoReader, VideoReader>();
            container.RegisterSingleton<IVideoDevicesResolver, VideoDevicesResolver>();
            container.RegisterSingleton<SkeletonizationContext>();
            container.RegisterScoped<IEmailSender, EmailSender>();
        }
    }
}
