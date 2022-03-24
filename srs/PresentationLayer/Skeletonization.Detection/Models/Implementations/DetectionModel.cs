using Emgu.CV;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Skeletonization.BusinessLayer.Abstractions;
using Skeletonization.CrossLayer.Data;
using Skeletonization.CrossLayer.Extensions;
using Skeletonization.PresentationLayer.Detection.Models.Abstractions;
using Skeletonization.PresentationLayer.Shared.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Skeletonization.PresentationLayer.Detection.Models.Implementations
{
    internal class DetectionModel : ReactiveObject, IDetectionModel, IVideoProcessingHandler
    {
        public IVideoService VideoService { get; }
        public IFinder Finder { get; }

        public ObservableCollection<Zone> Zones { get; } = new();
        [Reactive] public byte[] FrameBytes { get; set; }

        [Reactive] public IEnumerable<Human> Humans { get; set; }

        public DetectionModel(IVideoService videoService, IFinder finder)
        {
            VideoService = videoService;
            Finder = finder;

            VideoService.StartCamera(0, this);
        }

        public async Task HandleFrame(Mat mat)
        {
            using var copy = mat.Clone();
            FrameBytes = copy.ToBytes();
        }

        public void HandleVideoInformation(Size size)
        {
            Init();
        }

        private async void Init()
        {
            await Task.Delay(1000);
            Zones.Add(new(0, 0, 0.1, 0.1));
            await Task.Delay(1000);
            Zones.Add(new(0, 0.05, 0.1, 0.1));
            await Task.Delay(1000);
            Zones.Add(new(0, 0.05, 0.1, 0.1));
        }
    }
}