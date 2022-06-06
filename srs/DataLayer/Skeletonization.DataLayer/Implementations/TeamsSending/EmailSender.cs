using Skeletonization.DataLayer.Abstractions;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Skeletonization.DataLayer.Implementations.TeamsSending
{
    internal class EmailSender : IEmailSender
    {
        public IEmailConfiguration EmailConfiguration { get; }

        public EmailSender(IEmailConfiguration emailConfiguration)
        {
            EmailConfiguration = emailConfiguration;
        }

        public async Task Send(string title, string description, string imageName, byte[] imageBytes)
        {
            var from = new MailAddress(EmailConfiguration.SelfMail, EmailConfiguration.SelfName);
            var to = new MailAddress(EmailConfiguration.RecieveMail);
            var credential = new NetworkCredential(EmailConfiguration.SelfMail, EmailConfiguration.SelfPassword);

            using var message = new MailMessage(from, to)
            {
                Subject = title,
                Body = description,
            };

            using var client = new SmtpClient(EmailConfiguration.Host, EmailConfiguration.Port)
            {
                Credentials = credential,
                EnableSsl = true,
            };

            using MemoryStream imageStream = new(imageBytes);
            message.Attachments.Add(new Attachment(imageStream, imageName));

            await client.SendMailAsync(message);
        }
    }
}
