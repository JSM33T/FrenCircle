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