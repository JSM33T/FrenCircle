using Dapper;
using FrenCircle.Contracts.Dtos.Requests;
using FrenCircle.Contracts.Dtos.Responses;
using FrenCircle.Contracts.Interfaces.Services;
using FrenCircle.Infra.Dapper;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace FrenCircle.Repositories
{
    public class ChangeLogRepository : IChangeLogRepository
    {
        private readonly IDapperFactory _dbFactory;

        public ChangeLogRepository(IDapperFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        [Authorize]
        public async Task<int> InsertChangeLogAsync(ChangeLogRequestDto dto)
        {
            using var conn = _dbFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@Version", dto.Version);
            parameters.Add("@Title", dto.Title);
            parameters.Add("@Description", dto.Description);
            parameters.Add("@ChangeType", dto.ChangeType);
            parameters.Add("@Contributors", dto.Contributors);

            var result = await conn.ExecuteScalarAsync<int>(
                "InsertChangeLog",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result; // returns Id of inserted/updated record
        }

        public async Task DeleteByVersionAsync(string version)
        {
            using var conn = _dbFactory.CreateConnection();

            await conn.ExecuteAsync(
                "DeleteChangeLogsByVersion",
                new { Version = version },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<VersionGroupedChangeLogDto>> GetGroupedByVersionAsync()
        {
            using var conn = _dbFactory.CreateConnection();

            var flatLogs = await conn.QueryAsync<ChangeLogResponseDto>(
                "SELECT * FROM ChangeLogs ORDER BY Version, ChangedAt"
            );

            var grouped = flatLogs
                .GroupBy(log => log.Version)
                .Select(g => new VersionGroupedChangeLogDto
                {
                    Version = g.Key,
                    Changes = g.ToList()
                });

            return grouped;
        }

    }
}
