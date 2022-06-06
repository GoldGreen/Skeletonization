namespace Skeletonization.DataLayer.Abstractions
{
    public interface IEmailConfiguration
    {
        string SelfMail { get; set; }
        string SelfPassword { get; set; }
        string SelfName { get; set; }
        string RecieveMail { get; set; }
        string Host { get; set; }
        int Port { get; set; }
    }
}
