using FrenCircle.Contracts.Dtos;
using FrenCircle.Contracts.Dtos.Requests;
using FrenCircle.Contracts.Dtos.Responses;

namespace FrenCircle.Contracts.Interfaces.Services
{
    public interface IAuthService
    {
        //Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
        Task<(LoginResponseDto Response, string RefreshToken)> LoginAsync(LoginRequestDto dto);
        Task<bool> SignupAsync(SignupUserDto dto);
        Task<IEnumerable<SessionDto>> GetUserSessionsAsync(int userId);
        Task<bool> LogoutSessionAsync(int sessionId);
        Task VerifyEmailAsync(Guid token);
        Task<(LoginResponseDto Response, string NewRefreshToken)> RefreshTokenAsync(string refreshToken, string deviceId);

    }
}
