using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace FrenCircle.Infra.Background
{
    public class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
    {
        private readonly Channel<(int JobId, Func<CancellationToken, Task>)> _queue = Channel.CreateUnbounded<(int, Func<CancellationToken, Task>)>();

        public async Task EnqueueAsync(Func<CancellationToken, Task> taskFunc, string jobName, string? triggeredBy = null, string? metadata = null)
        {
            using var scope = serviceProvider.CreateScope();
            var jobHistoryRepo = scope.ServiceProvider.GetRequiredService<IJobHistoryRepository>();

            var job = new JobHistory
            {
                JobName = jobName,
                Status = "Pending",
                ScheduledAt = DateTime.UtcNow,
                TriggeredBy = triggeredBy,
                Metadata = metadata
            };

            var jobId = await jobHistoryRepo.AddAsync(job);
            await _queue.Writer.WriteAsync((jobId, taskFunc));
        }

        public async Task<(int JobId, Func<CancellationToken, Task>)> DequeueAsync(CancellationToken cancellationToken)
        {
            return await _queue.Reader.ReadAsync(cancellationToken);
        }
    }
}
