namespace API.Contracts.Services
{
    /// <summary>
    /// Represents a service for sending and managing emails.
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Sends multiple email messages asynchronously with specified sender information.
        /// </summary>
        /// <param name="recipients">A list of recipient email addresses.</param>
        /// <param name="recipients">A list of recipient email addresses.</param>
        /// <param name="subject">The subject of the emails.</param>
        /// <param name="body">The body content of the emails.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SendEmailsAsync(IEnumerable<string> recipients, string subject, string body);
    }
}
