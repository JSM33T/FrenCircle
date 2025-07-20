using Microsoft.EntityFrameworkCore;

namespace FrenCircle.Data.Repositories
{
    public interface IJobRepository
    {
        /// <summary>
        /// Retrieves a job by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the job.</param>
        /// <returns>The job entity if found; otherwise, null.</returns>
        Task<Job?> GetJobByIdAsync(int id);

        /// <summary>
        /// Updates the status of a job.
        /// </summary>
        /// <param name="id">The unique identifier of the job.</param>
        /// <param name="status">The new status to set for the job.</param>
        Task UpdateJobStatusAsync(int id, JobStatus status);

        /// <summary>
        /// Updates the job to mark it as started and sets the start date.
        /// </summary>
        /// <param name="id">The unique identifier of the job.</param>
        Task UpdateJobOnStartAsync(int id);

        /// <summary>
        /// Updates the job to mark it as completed, sets the completion date, and optionally records an error message.
        /// </summary>
        /// <param name="id">The unique identifier of the job.</param>
        /// <param name="status">The final status of the job.</param>
        /// <param name="errorMessage">An optional error message if the job failed.</param>
        Task UpdateJobOnCompletionAsync(int id, JobStatus status, string? errorMessage = null);

        /// <summary>
        /// Updates the job on retry, increments the retry count, and records the error message.
        /// </summary>
        /// <param name="id">The unique identifier of the job.</param>
        /// <param name="errorMessage">The error message for the retry attempt.</param>
        Task UpdateJobOnRetryAsync(int id, string errorMessage);
    }

    public class JobRepository : IJobRepository
    {
        private readonly AuthDbContext _context;

        public JobRepository(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<Job?> GetJobByIdAsync(int id)
        {
            return await _context.Jobs.FindAsync(id);
        }

        public async Task UpdateJobStatusAsync(int id, JobStatus status)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job != null)
            {
                job.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateJobOnStartAsync(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job != null)
            {
                job.Status = JobStatus.Running;
                job.StartedDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateJobOnCompletionAsync(int id, JobStatus status, string? errorMessage = null)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job != null)
            {
                job.Status = status;
                job.CompletionDate = DateTime.UtcNow;
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    job.ErrorMessage = errorMessage;
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateJobOnRetryAsync(int id, string errorMessage)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job != null)
            {
                job.ErrorMessage = errorMessage;
                job.CompletionDate = DateTime.UtcNow;
                job.RetryCount++;

                if (job.RetryCount < job.MaxRetries)
                {
                    job.Status = JobStatus.Pending;
                    job.CompletionDate = null;
                }
                else
                {
                    job.Status = JobStatus.Failed;
                }
                
                await _context.SaveChangesAsync();
            }
        }
    }
}
