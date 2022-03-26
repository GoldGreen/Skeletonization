using Prism.Services.Dialogs;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Skeletonization.CrossLayer.Data;
using Skeletonization.DataLayer.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;

namespace Skeletonization.PresentationLayer.Detection.ViewModels
{
    internal class OpenCameraDialogViewModel : ReactiveObject, IDialogAware
    {
        public event Action<IDialogResult> RequestClose;
        public string Title => "Выбор камеры";

        public IVideoDevicesResolver VideoDevicesResolver { get; }

        [Reactive] public IEnumerable<VideoDeviceInfo> VideoDevices { get; set; }
        [Reactive] public VideoDeviceInfo SelectedDevice { get; set; }

        public ICommand CompleteCommand { get; }
        public ICommand CancelCommand { get; }

        public OpenCameraDialogViewModel(IVideoDevicesResolver videoDevicesResolver)
        {
            VideoDevicesResolver = videoDevicesResolver;
            VideoDevices = VideoDevicesResolver.ResolveVideoDevices();

            CompleteCommand = ReactiveCommand.Create(() =>
            {
                DialogParameters parameters = new();
                parameters.Add("device", SelectedDevice);
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK, parameters));
            }, this.WhenAnyValue(x => x.SelectedDevice).Select(x => x is not null));

            CancelCommand = ReactiveCommand.Create(() => RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel)));
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }
    }
}
