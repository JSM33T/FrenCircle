using System.Net.Mail;
using FrenCircle.Entities;
using Microsoft.Extensions.Options;

namespace FrenCircle.Infra;

    public interface IEmailService
    {
        Task<bool> SendEmailAsync(List<string> recipients, string subject, string body);
    }

    public class EmailService(IOptions<FcConfig> config) : IEmailService
    {
        private readonly FcConfig _config = config.Value;
        
        public async Task<bool> SendEmailAsync(List<string> recipients, string subject, string body)
        {
            using var smtpClient = new SmtpClient(_config.SmtpSettings.Server, _config.SmtpSettings.Port);
            smtpClient.EnableSsl = false;
            smtpClient.Credentials = new System.Net.NetworkCredential(_config.SmtpSettings.Username, _config.SmtpSettings.Password);
            
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_config.SmtpSettings.FromEmail, _config.SmtpSettings.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            // Add all recipients
            foreach (var recipient in recipients)
            {
                mailMessage.To.Add(recipient);
            }

            await smtpClient.SendMailAsync(mailMessage);
            return true;
        }
    }
