using FrenCircle.Contracts.Dtos.Requests;
using FrenCircle.Contracts.Dtos.Responses;
using FrenCircle.Contracts.Interfaces.Repositories;
using FrenCircle.Contracts.Interfaces.Services;

namespace FrenCircle.Application
{
    public class ChangeLogService : IChangeLogService
    {
        private readonly IChangeLogRepository _repository;

        public ChangeLogService(IChangeLogRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> AddBulkChangeLogsAsync(ChangeLogBulkRequestDto bulkDto)
        {
            int insertedCount = 0;

            await _repository.DeleteByVersionAsync(bulkDto.Version);

            foreach (var change in bulkDto.Changes)
            {
                var dto = new ChangeLogRequestDto
                {
                    Version = bulkDto.Version,
                    Title = change.Title,
                    Description = change.Description,
                    ChangeType = change.ChangeType,
                    Contributors = change.Contributors
                };

                await _repository.InsertChangeLogAsync(dto);
                insertedCount++;
            }

            return insertedCount;
        }

        public async Task<IEnumerable<VersionGroupedChangeLogDto>> GetGroupedByVersionAsync()
        {
            return await _repository.GetGroupedByVersionAsync();
        }

    }
}
