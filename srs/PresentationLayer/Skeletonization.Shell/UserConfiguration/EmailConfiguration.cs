using Skeletonization.DataLayer.Abstractions;

namespace Skeletonization.PresentationLayer.Shell.UserConfiguration
{
    internal class EmailConfiguration : IEmailConfiguration
    {
        public string SelfMail
        {
            get => Settings.Default.SelfMail;
            set
            {
                if (SelfMail == value)
                {
                    return;
                }
                Settings.Default.SelfMail = value;
                Settings.Default.Save();
            }
        }

        public string SelfPassword
        {
            get => Settings.Default.SelfPassword;
            set
            {
                if (SelfPassword == value)
                {
                    return;
                }
                Settings.Default.SelfPassword = value;
                Settings.Default.Save();
            }
        }
        public string SelfName
        {
            get => Settings.Default.SelfName;
            set
            {
                if (SelfName == value)
                {
                    return;
                }
                Settings.Default.SelfName = value;
                Settings.Default.Save();
            }
        }
        public string RecieveMail
        {
            get => Settings.Default.RecieveMail;
            set
            {
                if (RecieveMail == value)
                {
                    return;
                }
                Settings.Default.RecieveMail = value;
                Settings.Default.Save();
            }
        }
        public string Host
        {
            get => Settings.Default.Host;
            set
            {
                if (Host == value)
                {
                    return;
                }
                Settings.Default.Host = value;
                Settings.Default.Save();
            }
        }
        public int Port
        {
            get => Settings.Default.Port;
            set
            {
                if (Port == value)
                {
                    return;
                }
                Settings.Default.Port = value;
                Settings.Default.Save();
            }
        }
    }
}
