using Emgu.CV.Dnn;
using Prism.Ioc;
using Skeletonization.BusinessLayer.Abstractions;
using Skeletonization.BusinessLayer.Configuration;
using Skeletonization.BusinessLayer.Implementation.Detection;
using Skeletonization.BusinessLayer.Implementation.Reading;
using Skeletonization.CrossfulLayer.Extensions;

namespace Skeletonization.BusinessLayer
{
    public static class LayerRegistration
    {
        public static void AddBusinesLayer(this IContainerRegistry container)
        {
            container.RegisterConfiguration();
            container.RegisterOption<NetOption>(NetOption.Net);

            container.RegisterNet();

            container.RegisterSingleton<IDetector, Detector>()
                     .RegisterSingleton<IPreparer, Preparer>()
                     .RegisterSingleton<IDrawer, Drawer>()
                     .RegisterSingleton<IHumanConverter, HumanConverter>()
                     .RegisterSingleton<IFinder, Finder>()
                     .RegisterSingleton<IVideoService, VideoService>();
        }

        private static void RegisterNet(this IContainerRegistry container)
        {
            container.RegisterSingleton<Net>(c =>
            {
                var netOption = c.Resolve<NetOption>();
                var net = DnnInvoke.ReadNetFromCaffe(netOption.Config, netOption.Model);

                if (netOption.UseCuda)
                {
                    net.SetPreferableBackend(Backend.Cuda);
                    net.SetPreferableTarget(Target.Cuda);
                }

                return net;
            });
        }
    }
}
