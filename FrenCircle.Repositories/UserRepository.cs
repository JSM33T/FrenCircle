using Dapper;
using FrenCircle.Entities.Fren;
using FrenCircle.Entities.Shared;
using FrenCircle.Repositories.Database;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;

namespace FrenCircle.Repositories
{
    public class UserRepository(ILogger<UserRepository> logger, IDbConnectionFactory dbConnectionFactory) : IUserRepository
    {
        protected readonly ILogger _logger = logger;
        private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

        public async Task<Fren?> GetUserByCredentials(FrenLoginRequest loginRequest)
        {
            using (IDbConnection db = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var parameters = new
                {
                    loginRequest.Username,
                    loginRequest.Password
                };

                return await db.QueryFirstOrDefaultAsync<Fren?>("sproc_FrenLogin", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<FrenLoginResponse?> LoginUser(FrenLoginRequest loginRequest)
        {
            var user = await GetUserByCredentials(loginRequest);

            if (user == null)
            {
                return null;
            }

            
            return new FrenLoginResponse
            {
                Username = user.Username,
                LastName = $"{user.FirstName} {user.LastName}",
            };
        }

        public async Task SignUpFren(FrenSignUpRequest signUpRequest)
        {
            using (IDbConnection db = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var parameters = new
                {
                    signUpRequest.Username,
                    signUpRequest.FirstName,
                    signUpRequest.LastName,
                    signUpRequest.Email,
                    signUpRequest.Password,
                    OTP = signUpRequest.OTP,
                    ValidTill = DateTime.Now.AddHours(10),
                };

                await db.ExecuteAsync("sproc_InsertFren", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> VerifyFren(FrenVerificationRequest verificationRequest)
        {
            using (IDbConnection db = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var parameters = new
                {
                    verificationRequest.Username,
                    verificationRequest.OTP
                };

                return await db.QueryFirstOrDefaultAsync<int>("sproc_VerifyFren", parameters, commandType: CommandType.StoredProcedure);
            }
        }

    }
}
