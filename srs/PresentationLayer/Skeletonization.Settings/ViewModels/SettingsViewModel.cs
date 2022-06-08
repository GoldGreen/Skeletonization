using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Skeletonization.PresentationLayer.Shared.Reactive;
using Skeletonization.Settings.Models.Abstractions;
using System.Reactive.Linq;
using System.Windows.Input;
using System;
using System.Windows.Controls;

namespace Skeletonization.Settings.ViewModels
{
    internal class SettingsViewModel : ReactiveObject
    {
        public ISettingsModel Model { get; }

        public ICommand ResetCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand PasswordChangedCommand { get; }

        [Reactive] public bool IsChanged { get; set; }

        public SettingsViewModel(ISettingsModel model)
        {
            Model = model;

            Model.WhenAnyValue(x => x.SelfMail,
                               x => x.SelfPassword,
                               x => x.SelfName,
                               x => x.RecieveMail,
                               x => x.Host,
                               x => x.Port)
                .Skip(1)
                .Subscribe(_ => IsChanged = true)
                .Cashe();

            ResetCommand = ReactiveCommand.Create(() =>
            {
                Model.Reset();
                IsChanged = false;
            }, this.WhenAnyValue(x => x.IsChanged));

            SaveCommand = ReactiveCommand.Create(() =>
            {
                Model.Save();
                IsChanged = false;
            }, this.WhenAnyValue(x => x.IsChanged));

            PasswordChangedCommand = ReactiveCommand.Create<string>(p =>
            Model.SelfPassword = p);
        }
    }
}
