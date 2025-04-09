using FrenCircle.Contracts.Models;

namespace FrenCircle.Contracts.Interfaces.Repositories
{
    public interface IJobHistoryRepository
    {
        Task<int> AddAsync(JobHistory job, CancellationToken cancellationToken = default);
        Task UpdateStatusAsync(int jobId, string status, DateTime? startedAt = null, DateTime? completedAt = null, int? durationMs = null, string? error = null, CancellationToken cancellationToken = default);
        Task<JobHistory?> GetByIdAsync(int jobId, CancellationToken cancellationToken = default);
        Task<IEnumerable<JobHistory>> GetRecentAsync(int count = 50, CancellationToken cancellationToken = default);
    }

}
