//using Microsoft.Extensions.Options;

//namespace FrenCircle.Infra.MailService.SendGrid
//{
//    internal class SendGridMailService : IMailService
//    {
//        private readonly SendGridConfig _config;
//        private readonly SendGridClient _client;

//        public SendGridMailService(IOptions<SendGridConfig> options)
//        {
//            _config = options.Value;
//            _client = new SendGridClient(_config.ApiKey);
//        }

//        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
//        {
//            var from = new EmailAddress(_config.FromEmail, _config.FromName);
//            var toEmail = new EmailAddress(to);
//            var msg = MailHelper.CreateSingleEmail(from, toEmail, subject, isHtml ? null : body, isHtml ? body : null);
//            await _client.SendEmailAsync(msg);
//        }

//        public async Task SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachment, string fileName, bool isHtml = true)
//        {
//            var from = new EmailAddress(_config.FromEmail, _config.FromName);
//            var toEmail = new EmailAddress(to);
//            var msg = MailHelper.CreateSingleEmail(from, toEmail, subject, isHtml ? null : body, isHtml ? body : null);

//            msg.AddAttachment(fileName, Convert.ToBase64String(attachment));
//            await _client.SendEmailAsync(msg);
//        }

//        public async Task SendBulkEmailAsync(IEnumerable<string> recipients, string subject, string body, bool isHtml = true)
//        {
//            foreach (var to in recipients)
//            {
//                await SendEmailAsync(to, subject, body, isHtml);
//            }
//        }
//    }
//}
