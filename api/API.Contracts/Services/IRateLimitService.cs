namespace API.Contracts.Services
{
    public interface IRateLimitService
    {
        /// <summary>
        /// Determines if a user is rate-limited based on the number of requests made within a specified time window.
        /// </summary>
        /// <param name="userId">The ID of the user whose requests are being checked.</param>
        /// <param name="rateLimitSeconds">The time window in seconds to consider for rate limiting.</param>
        /// <returns>A boolean indicating whether the user is currently rate-limited.</returns>
        public bool IsRateLimited(string userId, int rateLimitSeconds);
    }
}
