using FrenCircle.Contracts.Dtos;
using FrenCircle.Contracts.Dtos.Requests;
using FrenCircle.Contracts.Models;

namespace FrenCircle.Contracts.Interfaces.Repositories
{
    public interface IAuthRepository
    {
        Task<UserLogin?> GetLoginDataByEmailAsync(string email);
        Task<int> InsertUserAsync(SignupUserDto dto, string passwordHash, string salt);
        Task<int> InsertUserLoginAsync(int userId, string email, string passwordHash, string salt);
        Task<int> CreateSessionAsync(LoginSession session);
        Task<IEnumerable<SessionDto>> GetSessionsByUserIdAsync(int userId);
        Task VerifyEmailTokenAsync(Guid token);
        //Task<(int UserId, int UserLoginId)> ValidateRefreshTokenAsync(string refreshToken, string deviceId);
        Task<(int UserId, int UserLoginId, int SessionId)> ValidateRefreshTokenAsync(string refreshToken, string deviceId);

        Task StoreNewRefreshTokenAsync(int sessionId, string newRefreshToken, DateTime expiresAt);

    }
}
