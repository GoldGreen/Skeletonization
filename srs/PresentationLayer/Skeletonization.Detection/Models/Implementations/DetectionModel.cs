using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Skeletonization.CrossfulLayer.Extensions;
using Skeletonization.DataLayer.Reading.Abstractions;
using Skeletonization.PresentationLayer.Detection.Models.Abstractions;

namespace Skeletonization.PresentationLayer.Detection.Models.Implementations
{
    internal class DetectionModel : ReactiveObject, IDetectionModel
    {
        public IVideoReader VideoReader { get; }
        [Reactive] public byte[] Frame { get; set; }

        public DetectionModel(IVideoReader videoReader)
        {
            VideoReader = videoReader;
            VideoReader.Start(async mat =>
            {
                Frame = mat.ToBytes();
            });
        }
    }
}