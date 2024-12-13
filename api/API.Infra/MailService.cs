using API.Contracts.Services;
using API.Entities.Shared;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace API.Infra
{
    /// <summary>
    /// Provides an implementation of the IMailService for sending emails.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="MailService"/> class.
    /// </remarks>
    /// <param name="smtpClient">The SMTP client used for sending emails.</param>
    public class MailService : IMailService
    {

        private readonly SmtpClient _smtpClient;
        private readonly SmtpSettings _config;

        // Constructor for minimal setup with just SMTP server and port
        public MailService(IOptionsMonitor<Jsm33tConfig> config)
        {
            _config = config.CurrentValue.SmtpSettings;

            _smtpClient = new SmtpClient(_config.Server, _config.Port)
            {
                Credentials = new System.Net.NetworkCredential(_config.Username, _config.Password),
                EnableSsl = _config.EnableSSL,
            };
        }

        /// <summary>
        /// Sends multiple email messages asynchronously.
        /// </summary>
        /// <param name="recipients">A list of recipient email addresses.</param>
        /// <param name="subject">The subject of the emails.</param>
        /// <param name="body">The body content of the emails.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendEmailsAsync(IEnumerable<string> recipients, string subject, string body)
        {
            if (recipients == null || !recipients.Any())
                throw new ArgumentNullException(nameof(recipients), "Recipient list cannot be null or empty.");

            var tasks = recipients.Select(to => SendEmailAsync(_config.FromEmail, to, subject, body));
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Sends a single email asynchronously.
        /// </summary>
        /// <param name="from">The sender email address.</param>
        /// <param name="to">The recipient email address.</param>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body content of the email.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task SendEmailAsync(string from, string to, string subject, string body)
        {
            using var mailMessage = new MailMessage(from, to, subject, body)
            {
                IsBodyHtml = true
            };

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
