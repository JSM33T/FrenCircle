using API.Contracts.Services;
using API.Entities.Dedicated;

namespace API.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IDataService _dataService;
        public MessageRepository(IDataService dataService)
        {
            _dataService = dataService;
        }
        public async Task<bool> AddReportAsync(AddReportRequest reportRequest)
        {
            var query = @"
                INSERT INTO [dbo].[tblMessages] 
                (Message, Source) 
                VALUES 
                (@Report, @Source)";

            await _dataService.ExecuteAsync(query, reportRequest);
            return true;
        }
    }
}
