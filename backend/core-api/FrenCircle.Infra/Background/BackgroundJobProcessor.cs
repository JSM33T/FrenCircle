using FrenCircle.Data;
using FrenCircle.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FrenCircle.Infra.Background
{
    public class BackgroundJobProcessor : BackgroundService
    {
        private readonly IDispatcher _dispatcher;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BackgroundJobProcessor> _logger;

        public BackgroundJobProcessor(
            IDispatcher dispatcher,
            IServiceProvider serviceProvider,
            ILogger<BackgroundJobProcessor> logger)
        {
            _dispatcher = dispatcher;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background job processor started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var (jobId, taskFunc) = await _dispatcher.DequeueAsync(stoppingToken);
                    await ProcessJobAsync(jobId, taskFunc, stoppingToken);
                }
                catch (OperationCanceledException)
                { 
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in background job processor");
                    await Task.Delay(5000, stoppingToken);
                }
            }

            _logger.LogInformation("Background job processor stopped");
        }

        private async Task ProcessJobAsync(int jobId, Func<CancellationToken, Task> taskFunc, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var jobRepository = scope.ServiceProvider.GetRequiredService<IJobRepository>();

            var job = await jobRepository.GetJobByIdAsync(jobId);
            if (job == null)
            {
                _logger.LogWarning("Job not found: {JobId}", jobId);
                return;
            }

            try
            {
                _logger.LogInformation("Processing job: {JobName} (ID: {JobId})", job.JobName, jobId);
                
                await taskFunc(cancellationToken);
                
                await jobRepository.UpdateJobOnCompletionAsync(jobId, JobStatus.Completed);
                
                _logger.LogInformation("Job completed successfully: {JobName} (ID: {JobId})", job.JobName, jobId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Job failed: {JobName} (ID: {JobId})", job.JobName, jobId);
                
                if (job.RetryCount < job.MaxRetries)
                {
                    _logger.LogInformation("Retrying job: {JobName} (ID: {JobId}), Attempt: {RetryCount}/{MaxRetries}", 
                        job.JobName, jobId, job.RetryCount + 1, job.MaxRetries);
                    
                    await jobRepository.UpdateJobOnRetryAsync(jobId, ex.Message);
                }
                else
                {
                    await jobRepository.UpdateJobOnCompletionAsync(jobId, FrenCircle.Data.JobStatus.Failed, ex.Message);
                }
            }
        }
    }
}
