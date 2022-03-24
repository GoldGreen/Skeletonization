using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Skeletonization.BusinessLayer.Abstractions;
using Skeletonization.CrossfulLayer.Extensions;
using Skeletonization.DataLayer.Reading.Abstractions;
using Skeletonization.PresentationLayer.Detection.Models.Abstractions;

namespace Skeletonization.PresentationLayer.Detection.Models.Implementations
{
    internal class DetectionModel : ReactiveObject, IDetectionModel
    {
        public IVideoReader VideoReader { get; }
        public IFinder Finder { get; }

        [Reactive] public byte[] Frame { get; set; }

        public DetectionModel(IVideoReader videoReader, IFinder finder)
        {
            VideoReader = videoReader;
            Finder = finder;
            VideoReader.Start(async mat =>
            {
                using var copy = mat.Clone();
                var persons = await Finder.Find(copy, copy);
                Frame = copy.ToBytes();
            });
        }
    }
}