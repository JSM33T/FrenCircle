using System.Security.Claims;

namespace FrenCircle.Infra;

public enum ClaimType
{
    UserId,
    UserName,
    Email
}

public interface IClaimsService
{
    string? GetClaim(ClaimsPrincipal user, ClaimType claimType);
    int GetUserId(ClaimsPrincipal user);
    string? GetUserName(ClaimsPrincipal user);
    string? GetEmail(ClaimsPrincipal user);
}

public class ClaimsService : IClaimsService
{
    public string? GetClaim(ClaimsPrincipal user, ClaimType claimType)
    {
        var claimValue = claimType switch
        {
            ClaimType.UserId => user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            ClaimType.UserName => user.FindFirst(ClaimTypes.Name)?.Value,
            ClaimType.Email => user.FindFirst(ClaimTypes.Email)?.Value,
            _ => null
        };
        return claimValue;
    }

    public int GetUserId(ClaimsPrincipal user)
    {
        var userId = GetClaim(user, ClaimType.UserId);
        return int.TryParse(userId, out var id) ? id : 0;
    }

    public string? GetUserName(ClaimsPrincipal user)
    {
        return GetClaim(user, ClaimType.UserName);
    }

    public string? GetEmail(ClaimsPrincipal user)
    {
        return GetClaim(user, ClaimType.Email);
    }
}