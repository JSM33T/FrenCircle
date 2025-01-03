namespace API.Contracts.Services
{
    public interface IRateLimitService
    {
        /// <summary>
        /// Determines if a user is rate-limited based on the number of requests made within a specified time window.
        /// </summary>
        /// <param name="identifier">The identifiers requests are being checked according to.</param>
        /// <param name="maxRequests">The maximum number of requests allowed within the time window.</param>
        /// <param name="timeWindowSeconds">The time window in seconds to consider for rate limiting (e.g., 60 for 1 minute).</param>
        /// <returns>A boolean indicating whether the user is currently rate-limited (true if exceeded the limit, otherwise false).</returns>

        public bool IsRateLimited(string identifier, int maxRequests, int timeWindowSeconds);
    }
}
