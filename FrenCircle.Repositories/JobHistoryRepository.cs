using Dapper;
using FrenCircle.Contracts.Interfaces.Repositories;
using FrenCircle.Contracts.Models;
using FrenCircle.Infra.Dapper;

namespace FrenCircle.Repositories
{
    public class JobHistoryRepository(IDapperFactory dapperFactory) : IJobHistoryRepository
    {
        public async Task<int> AddAsync(JobHistory job, CancellationToken cancellationToken = default)
        {
            const string sql = @"
            INSERT INTO JobHistory (JobName, Status, ScheduledAt, StartedAt, CompletedAt, DurationMs, Error, TriggeredBy, Metadata)
            VALUES (@JobName, @Status, @ScheduledAt, @StartedAt, @CompletedAt, @DurationMs, @Error, @TriggeredBy, @Metadata);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using var conn = dapperFactory.CreateConnection();
            return await conn.ExecuteScalarAsync<int>(sql, job);
        }

        public async Task UpdateStatusAsync(int jobId, string status, DateTime? startedAt = null, DateTime? completedAt = null, int? durationMs = null, string? error = null, CancellationToken cancellationToken = default)
        {
            const string sql = @"
            UPDATE JobHistory
            SET Status = @Status,
                StartedAt = ISNULL(@StartedAt, StartedAt),
                CompletedAt = ISNULL(@CompletedAt, CompletedAt),
                DurationMs = ISNULL(@DurationMs, DurationMs),
                Error = ISNULL(@Error, Error)
            WHERE Id = @JobId";

            using var conn = dapperFactory.CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                JobId = jobId,
                Status = status,
                StartedAt = startedAt,
                CompletedAt = completedAt,
                DurationMs = durationMs,
                Error = error
            });
        }

        public async Task<JobHistory?> GetByIdAsync(int jobId, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT * FROM JobHistory WHERE Id = @JobId";

            using var conn = dapperFactory.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<JobHistory>(sql, new { JobId = jobId });
        }

        public async Task<IEnumerable<JobHistory>> GetRecentAsync(int count = 50, CancellationToken cancellationToken = default)
        {
            const string sql = @"
            SELECT TOP (@Count) *
            FROM JobHistory
            ORDER BY ScheduledAt DESC";

            using var conn = dapperFactory.CreateConnection();
            return await conn.QueryAsync<JobHistory>(sql, new { Count = count });
        }
    }

}
