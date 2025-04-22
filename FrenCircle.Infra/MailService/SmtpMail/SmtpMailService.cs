using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace FrenCircle.Infra.MailService.SmtpMail
{
    public class SmtpMailService : IMailService
    {
        private readonly SmtpConfig _config;

        public SmtpMailService(IOptions<SmtpConfig> smtpOptions) => _config = smtpOptions.Value;

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            using var message = new MailMessage(_config.From, to, subject, body)
            {
                IsBodyHtml = isHtml
            };

            using var client = BuildSmtpClient();
            await client.SendMailAsync(message);
        }

        public async Task SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachment, string fileName, bool isHtml = true)
        {
            using var message = new MailMessage(_config.From, to, subject, body)
            {
                IsBodyHtml = isHtml
            };

            message.Attachments.Add(new Attachment(new MemoryStream(attachment), fileName));

            using var client = BuildSmtpClient();
            await client.SendMailAsync(message);
        }

        public async Task SendBulkEmailAsync(IEnumerable<string> recipients, string subject, string body, bool isHtml = true)
        {
            foreach (var to in recipients)
            {
                await SendEmailAsync(to, subject, body, isHtml);
            }
        }

        private SmtpClient BuildSmtpClient()
        {
            return new SmtpClient(_config.Host, _config.Port)
            {
                Credentials = new NetworkCredential(_config.Username, _config.Password),
                EnableSsl = _config.EnableSsl
            };
        }
    }
}
