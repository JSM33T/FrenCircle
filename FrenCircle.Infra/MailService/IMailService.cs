namespace FrenCircle.Infra.MailService
{
    /// <summary>
    /// Provides functionalities to send emails including single, bulk, and with attachments.
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Sends a basic email.
        /// </summary>
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);

        /// <summary>
        /// Sends an email with an attachment.
        /// </summary>
        Task SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachment, string fileName, bool isHtml = true);

        /// <summary>
        /// Sends the same email to multiple recipients.
        /// </summary>
        Task SendBulkEmailAsync(IEnumerable<string> recipients, string subject, string body, bool isHtml = true);
    }
}
