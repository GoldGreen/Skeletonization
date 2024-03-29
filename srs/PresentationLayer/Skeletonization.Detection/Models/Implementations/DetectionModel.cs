﻿using Emgu.CV;
using Prism.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog;
using Skeletonization.BusinessLayer.Abstractions;
using Skeletonization.BusinessLayer.Data;
using Skeletonization.DataLayer.Data;
using Skeletonization.PresentationLayer.Detection.Models.Abstractions;
using Skeletonization.PresentationLayer.Shared.Data;
using Skeletonization.PresentationLayer.Shared.Extensions;
using Skeletonization.PresentationLayer.Shared.Prism;
using Skeletonization.PresentationLayer.Shared.Reactive;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Skeletonization.PresentationLayer.Detection.Models.Implementations
{
    internal class DetectionModel : ReactiveObject, IDetectionModel
    {
        public IVideoService VideoService { get; }
        public IFinder Finder { get; }
        public IDrawer Drawer { get; }
        public IEventAggregator EventAggregator { get; set; }
        public ILogger Logger { get; }
        [Reactive] public int FrameNum { get; set; }
        [Reactive] public long FrameHandlingTime { get; set; }
        [Reactive] public string VideoDescription { get; set; }

        [Reactive] public Mat Frame { get; set; }
        [Reactive] public Mat DrawedFrame { get; set; }
        [Reactive] public ImageSource FrameSource { get; set; }
        [Reactive] public bool Paused { get; set; } = false;

        [Reactive] public IEnumerable<HumanWithRoi> Humans { get; set; }


        public DetectionModel(IVideoService videoService,
                              IFinder finder,
                              IDrawer drawer,
                              IEventAggregator eventAggregator,
                              ILogger logger)
        {
            VideoService = videoService;
            Finder = finder;
            Drawer = drawer;
            EventAggregator = eventAggregator;
            Logger = logger;
            this.WhenAnyValue(x => x.DrawedFrame)
                .WhereNotNull()
                .Subscribe(x => FrameSource = x.ToImageSource())
                .Cashe();

            this.WhenAnyValue(x => x.Frame)
                .WhereNotNull()
                .Subscribe(EventAggregator.GetEvent<FrameChanged>().Publish)
                .Cashe();

            this.WhenAnyValue(x => x.Humans)
                .WhereNotNull()
                .Subscribe(EventAggregator.GetEvent<HumansChanged>().Publish)
                .Cashe();
        }

        public void StartVideoFromCamera(int cameraId)
        {
            try
            {
                Logger.Information("Открытие видео с камеры {id}", cameraId);
                VideoService.StartCamera(cameraId, this);
                VideoDescription = $"Камера: {cameraId}";
            }
            catch (Exception e)
            {
                Logger.Error("Ошибка открытия видео с камеры {id}", cameraId);
                EventAggregator.GetEvent<NotificationSended>().Publish(e.Message);
            }
        }

        public void StartVideoFromFile(string fileName)
        {
            try
            {
                Logger.Information("Открытие видеофайла: {path}", fileName);
                VideoService.StartFile(fileName, this);
                VideoDescription = $"Файл: {fileName}";
            }
            catch (Exception e)
            {
                Logger.Error("Ошибка открытия видеофайла: {path}", fileName);
                EventAggregator.GetEvent<NotificationSended>().Publish(e.Message);
            }
        }

        public Task HandleFrame(FrameInfo frame)
        {
            return Application.Current?.Dispatcher?.Invoke(async () =>
            {
                try
                {
                    while (Paused)
                    {
                        await Task.Delay(100);
                    }

                    var st = Stopwatch.StartNew();
                    var humans = await Finder.Find(frame.Mat);

                    Frame?.Dispose();
                    Frame = null;
                    DrawedFrame?.Dispose();
                    DrawedFrame = null;

                    Frame = frame.Mat;
                    var copy = frame.Mat.Clone();

                    if (humans.Count > 0)
                    {
                        Drawer.Draw(copy, humans);
                    }
                    DrawedFrame = copy;

                    Humans = humans.Select(x => Convert(x, Frame))
                                   .ToList();

                    FrameNum = frame.Num;
                    FrameHandlingTime = st.ElapsedMilliseconds;
                }
                catch (Exception e)
                {
                    Logger.Error("Ошибка обработки кадра {num} {mes}", frame.Num, e.Message);
                    EventAggregator.GetEvent<NotificationSended>().Publish(e.Message);
                }
            });
        }

        private static HumanWithRoi Convert(Human human, Mat mat)
        {
            var points = human.Points.Select
                               (bp => bp with
                               {
                                   Point = new
                                   (
                                       bp.Point.X / mat.Width,
                                       bp.Point.Y / mat.Height
                                   )
                               }).ToList();

            return new HumanWithRoi(mat.GetRoi(points.Select(x => x.Point))?.ToImageSource(),
                                    human.Name,
                                    points);
        }

        public void HandleVideoInformation(VideoInfo videoInfo)
        {
        }
    }
}