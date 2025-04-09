using FrenCircle.Contracts.Interfaces.Repositories;
using FrenCircle.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace FrenCircle.Infra.Background
{
    public class Dispatcher(IJobHistoryRepository jobHistoryRepository) : IDispatcher
    {
        private readonly IJobHistoryRepository _jobHistoryRepo = jobHistoryRepository;

        private readonly Channel<(int JobId, Func<CancellationToken, Task> Task)> _queue =
            Channel.CreateUnbounded<(int, Func<CancellationToken, Task>)>();

        public Task<(int JobId, Func<CancellationToken, Task> Task)> DequeueAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task EnqueueAsync(Func<CancellationToken, Task> taskFunc, string jobName, string? triggeredBy = null, string? metadata = null)
        {
            var job = new JobHistory
            {
                JobName = jobName,
                Status = "Pending",
                ScheduledAt = DateTime.UtcNow,
                TriggeredBy = triggeredBy,
                Metadata = metadata
            };

            var jobId = await _jobHistoryRepo.AddAsync(job);

            await _queue.Writer.WriteAsync((jobId, taskFunc));
        }
    }

}
