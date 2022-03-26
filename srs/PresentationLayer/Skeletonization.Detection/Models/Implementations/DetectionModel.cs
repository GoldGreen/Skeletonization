using Emgu.CV;
using Prism.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Skeletonization.BusinessLayer.Abstractions;
using Skeletonization.CrossfulLayer.Data;
using Skeletonization.CrossLayer.Data;
using Skeletonization.CrossLayer.Extensions;
using Skeletonization.PresentationLayer.Detection.Models.Abstractions;
using Skeletonization.PresentationLayer.Shared.Prism;
using Skeletonization.PresentationLayer.Shared.Reactive;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Skeletonization.PresentationLayer.Detection.Models.Implementations
{
    internal class DetectionModel : ReactiveObject, IDetectionModel
    {
        public IVideoService VideoService { get; }
        public IFinder Finder { get; }
        public IEventAggregator EventAggregator { get; set; }

        [Reactive] public int FrameNum { get; set; }
        [Reactive] public long FrameHandlingTime { get; set; }
        [Reactive] public string VideoDescription { get; set; }

        [Reactive] public Mat Frame { get; set; }
        [Reactive] public byte[] FrameBytes { get; set; }

        [Reactive] public IEnumerable<Human> Humans { get; set; }

        public DetectionModel(IVideoService videoService,
                              IFinder finder,
                              IEventAggregator eventAggregator)
        {
            VideoService = videoService;
            Finder = finder;
            EventAggregator = eventAggregator;

            this.WhenAnyValue(x => x.Frame)
                .WhereNotNull()
                .Do(x => FrameBytes = x.ToBytes())
                .Subscribe(EventAggregator.GetEvent<FrameChanged>().Publish)
                .Cashe();
        }

        public void StartVideoFromCamera(int cameraId)
        {
            VideoService.StartCamera(cameraId, this);
            VideoDescription = $"Камера: {cameraId}";
        }

        public void StartVideoFromFile(string fileName)
        {
            VideoService.StartFile(fileName, this);
            VideoDescription = $"Файл: {fileName}";
        }

        public Task HandleFrame(FrameInfo frame)
        {
            return Application.Current?.Dispatcher?.Invoke(async () =>
            {
                var st = Stopwatch.StartNew();

                await Task.Delay(100);
                Frame?.Dispose();
                Frame = null;
                Frame = frame.Mat;

                FrameNum = frame.Num;
                FrameHandlingTime = st.ElapsedMilliseconds;
            });
        }

        public void HandleVideoInformation(VideoInfo size)
        {
        }
    }
}