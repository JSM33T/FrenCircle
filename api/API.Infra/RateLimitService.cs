using API.Contracts.Services;

namespace API.Infra
{
    public class RateLimitService : IRateLimitService
    {
        private static readonly Dictionary<string, Dictionary<string, DateTime>> UserSpecificAccessTimes = new();
        private static readonly object Lock = new();

        public bool IsRateLimited(string userId, int maxRequests, int timeWindowSeconds)
        {
            var now = DateTime.UtcNow;

            lock (Lock)
            {
                if (!UserSpecificAccessTimes.TryGetValue(userId, out var userAccessTimes))
                {
                    userAccessTimes = new Dictionary<string, DateTime>();
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
