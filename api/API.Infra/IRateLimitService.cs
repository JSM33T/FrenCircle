using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Infra
{
    public interface IRateLimitService
    {
        public bool IsRateLimited(string userId, int rateLimitSeconds); 
    }
}
