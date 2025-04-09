using FrenCircle.Contracts.Dtos;
using FrenCircle.Contracts.Dtos.Requests;
using FrenCircle.Contracts.Dtos.Responses;
using FrenCircle.Contracts.Interfaces.Repositories;
using FrenCircle.Contracts.Interfaces.Services;
using FrenCircle.Contracts.Models;
using FrenCircle.Infra.Token;
using FrenCircle.Shared.Helpers;

namespace FrenCircle.Application
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repo;
        private readonly ITokenService _tokenService;

        public AuthService(IAuthRepository repo, ITokenService tokenService)
        {
            _repo = repo;
            _tokenService = tokenService;
        }

        public async Task<int> SignupAsync(SignupUserDto dto)
        {
            var salt = PasswordHelper.GenerateSalt();
            var hash = PasswordHelper.HashPassword(dto.Password, salt);
            return await _repo.InsertUserAsync(dto, hash, salt);
        }

        public async Task<(LoginResponseDto Response, string RefreshToken)> LoginAsync(LoginRequestDto dto)
        {
            var login = await _repo.GetLoginDataByEmailAsync(dto.Email);
            if (login == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            if (!PasswordHelper.VerifyPassword(dto.Password, login.PasswordHash!, login.Salt!))
                throw new UnauthorizedAccessException("Invalid credentials");

            var tokens = _tokenService.GenerateTokens(login.UserId);

            var session = new LoginSession
            {
                UserLoginId = login.Id,
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
                IssuedAt = tokens.IssuedAt,
                ExpiresAt = tokens.ExpiresAt,
                DeviceId = Guid.Parse(dto.DeviceId),
                IpAddress = dto.IpAddress,
                UserAgent = dto.UserAgent
            };

            var sessionId = await _repo.CreateSessionAsync(session);

            var response = new LoginResponseDto
            {
                UserId = login.UserId,
                SessionId = sessionId,
                AccessToken = tokens.AccessToken,
                ExpiresAt = tokens.ExpiresAt
            };

            return (response, tokens.RefreshToken);
        }

        public async Task<IEnumerable<SessionDto>> GetUserSessionsAsync(int userId)
        {
            return await _repo.GetSessionsByUserIdAsync(userId);
        }

        public async Task<bool> LogoutSessionAsync(int sessionId)
        {
            // Optional: implement stored proc to set IsActive = 0, LoggedOutAt = GETDATE()
            return true;
        }
        public async Task VerifyEmailAsync(Guid token)
        {
            await _repo.VerifyEmailTokenAsync(token);
        }

        public async Task<(LoginResponseDto Response, string NewRefreshToken)> RefreshTokenAsync(string refreshToken, string deviceId)
        {
            var (userId, userLoginId, sessionId) = await _repo.ValidateRefreshTokenAsync(refreshToken, deviceId);

            var tokens = _tokenService.GenerateTokens(userId);

            await _repo.StoreNewRefreshTokenAsync(sessionId, tokens.RefreshToken, tokens.ExpiresAt);

            var response = new LoginResponseDto
            {
                UserId = userId,
                SessionId = sessionId,
                AccessToken = tokens.AccessToken,
                ExpiresAt = tokens.ExpiresAt
            };

            return (response, tokens.RefreshToken);
        }


    }

}
