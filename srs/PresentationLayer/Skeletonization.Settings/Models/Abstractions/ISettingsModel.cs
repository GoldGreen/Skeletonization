using ReactiveUI;

namespace Skeletonization.Settings.Models.Abstractions
{
    public interface ISettingsModel : IReactiveObject
    {
        string SelfMail { get; set; }
        string SelfPassword { get; set; }
        string SelfName { get; set; }
        string RecieveMail { get; set; }
        string Host { get; set; }
        int Port { get; set; }

        void Save();
        void Reset();
    }
}
