
using API.Contracts.Services;
using Microsoft.AspNetCore.Http;

namespace API.Infra
{
    public class CommonService : ICommonService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommonService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value;
        }

        public string GetUsername()
        {
            return _httpContextAccessor.HttpContext?.User.Identity.Name;
        }
    }
}
