using FrenCircle.Contracts.Mail;

namespace FrenCircle.Infra.Repositories.Interfaces
{
    public interface IMailRepository
    {
        /// <summary>
        /// Sends a single email to a recipient
        /// </summary>
        /// <param name="emailMessage">The email message to send</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Email result with success status and details</returns>
        Task<EmailResult> SendEmailAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends emails to multiple recipients in batches
        /// </summary>
        /// <param name="bulkEmailMessage">The bulk email message with multiple recipients</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Bulk email result with overall status and individual results</returns>
        Task<BulkEmailResult> SendBulkEmailAsync(BulkEmailMessage bulkEmailMessage, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends an email using a predefined template
        /// </summary>
        /// <param name="templateType">The type of email template to use</param>
        /// <param name="recipient">The recipient's email and name</param>
        /// <param name="variables">Variables to replace in the template</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Email result with success status and details</returns>
        Task<EmailResult> SendTemplateEmailAsync(
            EmailTemplateType templateType, 
            EmailRecipient recipient, 
            Dictionary<string, string> variables, 
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends template emails to multiple recipients
        /// </summary>
        /// <param name="templateType">The type of email template to use</param>
        /// <param name="recipients">List of recipients with their personalization data</param>
        /// <param name="globalVariables">Global variables applied to all emails</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Bulk email result with overall status and individual results</returns>
        Task<BulkEmailResult> SendBulkTemplateEmailAsync(
            EmailTemplateType templateType, 
            List<EmailRecipient> recipients, 
            Dictionary<string, string>? globalVariables = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates email settings and connectivity
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if email service is properly configured and accessible</returns>
        Task<bool> ValidateEmailServiceAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets available email templates
        /// </summary>
        /// <returns>Dictionary of template types and their configurations</returns>
        Dictionary<EmailTemplateType, EmailTemplate> GetAvailableTemplates();

        /// <summary>
        /// Registers or updates an email template
        /// </summary>
        /// <param name="templateType">The template type</param>
        /// <param name="template">The template configuration</param>
        void RegisterTemplate(EmailTemplateType templateType, EmailTemplate template);
    }
}
