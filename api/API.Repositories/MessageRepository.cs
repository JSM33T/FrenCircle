using API.Contracts.Repositories;
using API.Contracts.Services;
using API.Entities.Dedicated.Contact;
using API.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IDataService _dataService;
        public MessageRepository(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<DBResult> AddContactMessage(ContactRequest request)
        {
            var parameters = new
            {
                UserId = request.Id,
                request.Message,
                request.Origin,
                request.Name
            };

            DBResult result = await _dataService.ExecuteStoredProcedureAsync(
                "sproc_AddMessage",
                parameters
            );


            return result;
        }
    }
}
