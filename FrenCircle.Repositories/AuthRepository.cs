using Dapper;
using FrenCircle.Contracts.Dtos.Requests;
using FrenCircle.Contracts.Dtos.Responses;
using FrenCircle.Contracts.Dtos;
using FrenCircle.Contracts.Interfaces.Repositories;
using FrenCircle.Contracts.Interfaces.Services;
using FrenCircle.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrenCircle.Infra.Dapper;

namespace FrenCircle.Repositories
{
    // AuthRepository.cs
    public class AuthRepository : IAuthRepository
    {
        private readonly IDbConnection _db;

        public AuthRepository(IDapperFactory factory)
        {
            _db = factory.CreateConnection();
        }

        public async Task<UserLogin?> GetLoginDataByEmailAsync(string email)
        {
            var result = await _db.QueryFirstOrDefaultAsync<UserLogin>(
                "EXEC GetUserLoginForEmail @Email",
                new { Email = email });
            return result;
        }

        public async Task<int> InsertUserAsync(SignupUserDto dto, string passwordHash, string salt)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@FirstName", dto.FirstName);
            parameters.Add("@LastName", dto.LastName);
            parameters.Add("@UserName", dto.UserName);
            parameters.Add("@Email", dto.Email);
            parameters.Add("@PasswordHash", passwordHash);
            parameters.Add("@Salt", salt);

            return await _db.ExecuteScalarAsync<int>("SignupUser", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> InsertUserLoginAsync(int userId, string email, string passwordHash, string salt)
        {
            // Not used currently; login is part of SignupUser SP
            throw new NotImplementedException();
        }

        public async Task<int> CreateSessionAsync(LoginSession session)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@UserLoginId", session.UserLoginId);
            parameters.Add("@AccessToken", session.AccessToken);
            parameters.Add("@RefreshToken", session.RefreshToken);
            parameters.Add("@IssuedAt", session.IssuedAt);
            parameters.Add("@ExpiresAt", session.ExpiresAt);
            parameters.Add("@DeviceId", session.DeviceId);
            parameters.Add("@IPAddress", session.IpAddress);
            parameters.Add("@UserAgent", session.UserAgent);

            return await _db.ExecuteScalarAsync<int>("CreateLoginSession", parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<(int UserId, int UserLoginId,int SessionId)> ValidateRefreshTokenAsync(string refreshToken, string deviceId)
        {
            const string sql = @"
            SELECT UL.UserId, UL.Id AS UserLoginId, LS.Id AS SessionId
            FROM LoginSessions LS
            INNER JOIN UserLogins UL ON LS.UserLoginId = UL.Id
            WHERE LS.RefreshToken = @RefreshToken
              AND LS.DeviceId = @DeviceId
              AND LS.IsActive = 1
              AND LS.ExpiresAt > GETUTCDATE();";

            var result = await _db.QueryFirstOrDefaultAsync<(int UserId, int UserLoginId,int SessionId)>(sql, new
            {
                RefreshToken = refreshToken,
                DeviceId = deviceId
            });

            if (result.UserId == 0)
                throw new UnauthorizedAccessException("Invalid refresh token or device ID.");

            return result;
        }

        public async Task<IEnumerable<SessionDto>> GetSessionsByUserIdAsync(int userId)
        {
            return await _db.QueryAsync<SessionDto>("EXEC GetUserSessions @UserId", new { UserId = userId });
        }
        public async Task VerifyEmailTokenAsync(Guid token)
        {
            await _db.ExecuteAsync("VerifyEmail", new { Token = token }, commandType: CommandType.StoredProcedure);
        }

        public async Task StoreNewRefreshTokenAsync(int sessionId, string newRefreshToken, DateTime expiresAt)
        {
            const string sql = @"
        UPDATE LoginSessions
        SET RefreshToken = @RefreshToken,
            ExpiresAt = @ExpiresAt
        WHERE Id = @SessionId;";

            await _db.ExecuteAsync(sql, new
            {
                SessionId = sessionId,
                RefreshToken = newRefreshToken,
                ExpiresAt = expiresAt
            });
        }

    }
}
