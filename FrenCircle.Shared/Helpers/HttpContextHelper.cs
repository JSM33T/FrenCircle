using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FrenCircle.Shared.Helpers
{
    public static class HttpContextHelper
    {
        public static int GetUserId(HttpContext context)
        {
            var claim = context?.User?.FindFirst(ClaimTypes.NameIdentifier);
            return int.TryParse(claim?.Value, out var id) ? id: 0;
        }
    }
}
