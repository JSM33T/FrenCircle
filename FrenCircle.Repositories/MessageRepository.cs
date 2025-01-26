using FrenCircle.Entities.Data;
using FrenCircle.Infra;
using FrenCircle.Helpers.SqlQueries;

namespace FrenCircle.Repositories
{
    public interface IMessageRepository
    {
        Task AddMessage(Message message);
        Task<List<Message>> GetAllMessages();
        Task<bool> IsMessagePresent(Message message);
    }
    public class MessageRepository(IDapperFactory dapperFactory) : IMessageRepository
    {

        private readonly IDapperFactory _dapperFactory = dapperFactory;

        public async Task AddMessage(Message message)
        {
            var query = DB_MESSAGE.ADD;

            int id = await _dapperFactory.GetData<int>(query, new
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
            var messages = await _dapperFactory.GetDataList<Message>(query);
            return messages.ToList();
        }

        public async Task<bool> IsMessagePresent(Message message)
        {
            var query = DB_MESSAGE.CHECK_BY_EMAIL;
            var existingMessage = await _dapperFactory.GetData<Message>(query, new { message.Email, message.Text });
            return existingMessage != null;
        }
    }
}
