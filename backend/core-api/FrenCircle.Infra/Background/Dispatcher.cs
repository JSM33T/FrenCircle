using FrenCircle.Data;
using FrenCircle.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Text.Json;

namespace FrenCircle.Infra.Background
{
    public class Dispatcher : IDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Dispatcher> _logger;
        private readonly ConcurrentQueue<(int JobId, Func<CancellationToken, Task> Task)> _taskQueue;

        public Dispatcher(IServiceProvider serviceProvider, ILogger<Dispatcher> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _taskQueue = new ConcurrentQueue<(int, Func<CancellationToken, Task>)>();
        }

        public async Task EnqueueAsync(Func<CancellationToken, Task> taskFunc, string jobName, string? triggeredBy = null, string? metadata = null)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

            var job = new Job
            {
                JobName = jobName,
                Status = JobStatus.Pending,
                Priority = JobPriority.Normal,
                CreatedDate = DateTime.UtcNow,
                TriggeredBy = triggeredBy,
                Metadata = metadata
            };

            dbContext.Jobs.Add(job);
            await dbContext.SaveChangesAsync();

            _taskQueue.Enqueue((job.Id, taskFunc));
            _logger.LogInformation("Job enqueued: {JobName} with ID: {JobId}", jobName, job.Id);
        }

        public async Task<(int JobId, Func<CancellationToken, Task>)> DequeueAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_taskQueue.TryDequeue(out var item))
                {
                    using var scope = _serviceProvider.CreateScope();
                    var jobRepository = scope.ServiceProvider.GetRequiredService<IJobRepository>();

                    await jobRepository.UpdateJobOnStartAsync(item.JobId);

                    return item;
                }

                await Task.Delay(1000, cancellationToken);
            }

            throw new OperationCanceledException();
        }
    }
}
