using API.Entities.Dedicated;

namespace API.Repositories
{
    public interface IMessageRepository
    {
        public Task<bool> AddReportAsync(AddReportRequest reportRequest);
    }
}
