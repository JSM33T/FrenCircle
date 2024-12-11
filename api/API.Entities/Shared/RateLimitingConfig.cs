using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Entities.Shared
{
    public class RateLimitingConfig
    {
        public RateLimitPolicyOptions Global { get; set; }
        public Dictionary<string, RateLimitPolicyOptions> Routes { get; set; }
    }
    public class RateLimitPolicyOptions
    {
        public int PermitLimit { get; set; }
        public TimeSpan Window { get; set; }
        public int QueueLimit { get; set; }
    }
}
