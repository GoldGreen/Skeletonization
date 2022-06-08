using Prism.Events;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Skeletonization.DataLayer.Abstractions;
using Skeletonization.PresentationLayer.Shared.Prism;
using Skeletonization.Settings.Models.Abstractions;
using System.Windows.Controls;

namespace Skeletonization.Settings.Models.Implementation
{
    internal class SettingsModel : ReactiveObject, ISettingsModel
    {
        [Reactive] public string SelfMail { get; set; }
        [Reactive] public string SelfPassword { get; set; }
        [Reactive] public string SelfName { get; set; }
        [Reactive] public string RecieveMail { get; set; }
        [Reactive] public string Host { get; set; }
        [Reactive] public int Port { get; set; }

        public IEmailConfiguration EmailConfiguration { get; }
        public IEventAggregator EventAggregator { get; }

        public SettingsModel(IEmailConfiguration emailConfiguration, IEventAggregator eventAggregator)
        {
            EmailConfiguration = emailConfiguration;
            EventAggregator = eventAggregator;
            Load();
        }

        public void Reset()
        {
            Load();
            EventAggregator.GetEvent<NotificationSended>().Publish("Настройки сброшены!");
        }

        public void Save()
        {
            EmailConfiguration.SelfMail = SelfMail;
            EmailConfiguration.SelfPassword = SelfPassword;
            EmailConfiguration.SelfName = SelfName;
            EmailConfiguration.RecieveMail = RecieveMail;
            EmailConfiguration.Host = Host;
            EmailConfiguration.Port = Port;

            EventAggregator.GetEvent<NotificationSended>().Publish("Настройки сохранены!");
        }

        private void Load()
        {
            SelfMail = EmailConfiguration.SelfMail;
            SelfPassword = EmailConfiguration.SelfPassword;
            SelfName = EmailConfiguration.SelfName;
            RecieveMail = EmailConfiguration.RecieveMail;
            Host = EmailConfiguration.Host;
            Port = EmailConfiguration.Port;
        }
    }
}
