using API.Entities.Dedicated.Contact;
using API.Entities.Enums;

namespace API.Contracts.Repositories
{
    /// <summary>
    /// Represents a repository for managing messages, including user inquiries or contact requests.
    /// </summary>
    public interface IMessageRepository
    {
        /// <summary>
        /// Adds a contact message to the database.
        /// </summary>
        /// <param name="request">The contact message details submitted by the user.</param>
        /// <returns>
        /// A task representing the asynchronous operation, with a <see cref="DBResult"/> 
        /// indicating the success or failure of the operation.
        /// </returns>
        Task<DBResult> AddContactMessage(ContactRequest request);
    }
}
