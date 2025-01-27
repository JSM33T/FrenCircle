using FrenCircle.Entities.Data;
using FrenCircle.Infra;
using FrenCircle.Helpers.SqlQueries;

namespace FrenCircle.Repositories
{
    /// <summary>
    /// Interface for a message repository to manage messages.
    /// </summary>
    public interface IMessageRepository
    {
        /// <summary>
        /// Adds a new message to the repository.
        /// </summary>
        /// <param name="message">The message to add.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task AddMessage(Message message);

        /// <summary>
        /// Retrieves all messages stored in the repository.
        /// </summary>
        /// <returns>A Task that returns a list of messages.</returns>
        Task<List<Message>> GetAllMessages();

        /// <summary>
        /// Checks if a specific message is present in the repository.
        /// </summary>
        /// <param name="message">The message to check for.</param>
        /// <returns>A Task that indicates whether the message is present or not.</returns>
        Task<bool> IsMessagePresent(Message message);
    }
    public class MessageRepository(IDapperFactory dapperFactory) : IMessageRepository
    {
        public async Task AddMessage(Message message)
        {
            var query = DB_MESSAGE.ADD;

            var id = await dapperFactory.GetData<int>(query, new
            {
                message.Name,
                message.Email,
                message.Text
            });

            message.Id = id;
        }

        public async Task<List<Message>> GetAllMessages()
        {
            var query = DB_MESSAGE.GETALL;
            var messages = await dapperFactory.GetDataList<Message>(query);
            return messages.ToList();
        }

        public async Task<bool> IsMessagePresent(Message message)
        {
            var query = DB_MESSAGE.CHECK_BY_EMAIL;
            var existingMessage = await dapperFactory.GetData<Message>(query, new { message.Email, message.Text });
            return existingMessage != null;
        }
    }
}
