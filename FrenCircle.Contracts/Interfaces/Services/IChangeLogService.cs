using FrenCircle.Contracts.Dtos.Requests;
using FrenCircle.Contracts.Dtos.Responses;

namespace FrenCircle.Contracts.Interfaces.Repositories
{
    public interface IChangeLogService
    {
        Task<int> AddBulkChangeLogsAsync(ChangeLogBulkRequestDto bulkDto);
        Task<IEnumerable<VersionGroupedChangeLogDto>> GetGroupedByVersionAsync();

    }
}
