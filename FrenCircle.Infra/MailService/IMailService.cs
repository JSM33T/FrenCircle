namespace FrenCircle.Infra.MailService
{
    /// <summary>
    /// Defines a service for sending emails, including support for plain, bulk, and attachment-based emails.
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Sends a basic email to a single recipient.
        /// </summary>
        /// <param name="to">Recipient email address.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="body">Body content of the email.</param>
        /// <param name="isHtml">Whether the body is HTML formatted.</param>
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);

        /// <summary>
        /// Sends an email with an attachment to a single recipient.
        /// </summary>
        /// <param name="to">Recipient email address.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="body">Body content of the email.</param>
        /// <param name="attachment">Attachment file as byte array.</param>
        /// <param name="fileName">Name of the attached file.</param>
        /// <param name="isHtml">Whether the body is HTML formatted.</param>
        Task SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachment, string fileName, bool isHtml = true);

        /// <summary>
        /// Sends the same email to multiple recipients.
        /// </summary>
        /// <param name="recipients">List of recipient email addresses.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="body">Body content of the email.</param>
        /// <param name="isHtml">Whether the body is HTML formatted.</param>
        Task SendBulkEmailAsync(IEnumerable<string> recipients, string subject, string body, bool isHtml = true);
    }
}