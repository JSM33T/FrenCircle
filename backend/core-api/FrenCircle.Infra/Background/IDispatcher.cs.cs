namespace FrenCircle.Infra
{
    /// <summary>
    /// Defines a dispatcher for managing background tasks with persistence support.
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// Enqueues a background task for execution and creates a JobHistory record.
        /// </summary>
        /// <param name="taskFunc">The task to execute.</param>
        /// <param name="jobName">A descriptive name for the job.</param>
        /// <param name="triggeredBy">Optional info about who or what triggered the job.</param>
        /// <param name="metadata">Optional metadata to store with the job.</param>
        Task EnqueueAsync(Func<CancellationToken, Task> taskFunc, string jobName, string? triggeredBy = null, string? metadata = null);

        /// <summary>
        /// Dequeues the next background task to execute, along with its associated Job ID.
        /// </summary>
        /// <param name="cancellationToken">Token to observe cancellation requests.</param>
        /// <returns>A tuple containing the Job ID and the task delegate.</returns>
        //Task<(int JobId, Func<CancellationToken, Task> Task)> DequeueAsync(CancellationToken cancellationToken);
        Task<(int JobId, Func<CancellationToken, Task>)> DequeueAsync(CancellationToken cancellationToken);
    }
}
