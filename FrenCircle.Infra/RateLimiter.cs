namespace FrenCircle.Infra
{
    public interface IRateLimiter
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
    public class RateLimiter : IRateLimiter
    {
        private static readonly Dictionary<string, Dictionary<string, DateTime>> UserSpecificAccessTimes = new();
        private static readonly Lock Lock = new();

        public bool IsRateLimited(string userId, int maxRequests, int timeWindowSeconds)
        {
            var now = DateTime.UtcNow;

            lock (Lock)
            {
                if (!UserSpecificAccessTimes.TryGetValue(userId, out var userAccessTimes))
                {
                    userAccessTimes = [];
                    UserSpecificAccessTimes[userId] = userAccessTimes;
                }

                // Remove expired requests based on the time window
                userAccessTimes = userAccessTimes
                    .Where(entry => now - entry.Value < TimeSpan.FromSeconds(timeWindowSeconds))
                    .ToDictionary(entry => entry.Key, entry => entry.Value);

                // Check if the number of requests exceeds the limit
                if (userAccessTimes.Count >= maxRequests)
                {
                    return true;
                }

                // Add the current request time
                userAccessTimes[Guid.NewGuid().ToString()] = now;
                UserSpecificAccessTimes[userId] = userAccessTimes;
            }

            return false;
        }

    }
}
