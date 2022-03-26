using Microsoft.Win32;
using Prism.Events;
using Prism.Services.Dialogs;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Skeletonization.PresentationLayer.Detection.Models.Abstractions;
using Skeletonization.PresentationLayer.Shared.Data;
using Skeletonization.PresentationLayer.Shared.Prism;
using Skeletonization.PresentationLayer.Shared.Reactive;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Linq;
using System.Windows.Input;

namespace Skeletonization.PresentationLayer.Detection.ViewModels
{
    internal class DetectionViewModel : ReactiveObject
    {
        public IDetectionModel Model { get; }
        public IEventAggregator EventAggregator { get; }
        public IDialogService DialogService { get; }
        public ObservableCollection<Zone> Zones { get; } = new();
        [Reactive] public Zone SelectedZone { get; set; }

        public ICommand StartVideoFromFileCommand { get; }
        public ICommand StartVideoFromCameraCommand { get; }

        public DetectionViewModel(IDetectionModel model,
                                  IEventAggregator eventAggregator,
                                  IDialogService dialogService)
        {
            Model = model;
            EventAggregator = eventAggregator;
            DialogService = dialogService;

            var zoneChanged = EventAggregator.GetEvent<ZonesChanged>()
                                             .ToObservable();

            zoneChanged.Where(x => x.action == NotifyCollectionChangedAction.Add)
                       .Select(x => x.zone)
                       .Subscribe(Zones.Add)
                       .Cashe();

            zoneChanged.Where(x => x.action == NotifyCollectionChangedAction.Remove)
                       .Select(x => x.zone)
                       .Subscribe(x => Zones.Remove(x))
                       .Cashe();

            this.WhenAnyValue(x => x.SelectedZone)
                .WhereNotNull()
                .Subscribe(x =>
                {
                    EventAggregator.GetEvent<ZoneSelected>().Publish(x);
                    SelectedZone = null;
                })
                .Cashe();

            StartVideoFromFileCommand = ReactiveCommand.Create(() =>
            {
                OpenFileDialog openFileDialog = new();

                string videoExtensions = "Видео".ConcatExtensions("AVI", "MP4", "MPEG", "MOV");
                string imageExtensions = "Изображения".ConcatExtensions("BMP", "JPEG", "JPG", "PNG");

                openFileDialog.Filter = string.Join('|', videoExtensions, imageExtensions);

                if (openFileDialog.ShowDialog() == true)
                {
                    Model.StartVideoFromFile(openFileDialog.FileName);
                }
            });

            StartVideoFromCameraCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var res = await DialogService.OpenCameraChooseDialog();
                if (res.ok)
                {
                    Model.StartVideoFromCamera(res.device.Id);
                }
            });
        }
    }
}
