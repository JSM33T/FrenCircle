using API.Contracts.Services;

namespace API.Infra
{
    public class RateLimitService : IRateLimitService
    {
        private static readonly Dictionary<string, Dictionary<string, DateTime>> UserSpecificAccessTimes = new();
        private static readonly object Lock = new();

        public bool IsRateLimited(string userId, int rateLimitSeconds)
        {
            var now = DateTime.UtcNow;

            lock (Lock)
            {
                if (!UserSpecificAccessTimes.TryGetValue(userId, out var userAccessTimes))
                {
                    userAccessTimes = new Dictionary<string, DateTime>();
                    UserSpecificAccessTimes[userId] = userAccessTimes;
                }

                if (userAccessTimes.TryGetValue("lastAccess", out var lastAccess))
                {
                    if (now - lastAccess < TimeSpan.FromSeconds(rateLimitSeconds))
                    {
                        return true;
                    }
                }

                userAccessTimes["lastAccess"] = now;
            }

            return false;
        }
    }
}
