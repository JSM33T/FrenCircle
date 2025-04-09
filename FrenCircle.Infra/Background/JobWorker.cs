using FrenCircle.Contracts.Interfaces.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FrenCircle.Infra.Background
{
    public class JobWorker(IDispatcher dispatcher, IJobHistoryRepository jobHistoryRepo, ILogger<JobWorker> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var (jobId, task) = await dispatcher.DequeueAsync(stoppingToken);
                var start = DateTime.UtcNow;

                try
                {
                    await jobHistoryRepo.UpdateStatusAsync(jobId, "Running", startedAt: start, cancellationToken: stoppingToken);
                    await task(stoppingToken);
                    var end = DateTime.UtcNow;
                    await jobHistoryRepo.UpdateStatusAsync(jobId, "Success", completedAt: end, durationMs: (int)(end - start).TotalMilliseconds, cancellationToken: stoppingToken);
                }
                catch (Exception ex)
                {
                    var failTime = DateTime.UtcNow;
                    await jobHistoryRepo.UpdateStatusAsync(jobId, "Failed", completedAt: failTime, error: ex.Message, cancellationToken: stoppingToken);
                    logger.LogError(ex, "Error occurred executing background task (JobId={JobId})", jobId);
                }
            }
        }
    }

}
