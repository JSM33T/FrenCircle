using Dapper;
using FrenCircle.Entities.Fren;
using FrenCircle.Entities.Shared;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenCircle.Repositories
{
    internal class UserRepository : IUserRepository
    {
        protected readonly IOptionsMonitor<FCConfig> _config;
        private readonly IDbConnection _dbConnection;
        protected readonly ILogger _logger;
        private string _conStr;

        public UserRepository(IOptionsMonitor<FCConfig> config, ILogger<UserRepository> logger, IDbConnection dbConnection)
        {
            _config = config;
            _logger = logger;
            _conStr = _config.CurrentValue.ConnectionString;
            _dbConnection = dbConnection;
        }


        public async Task<Fren?> GetUserByCredentials(FrenLoginRequest loginRequest)
        {
            Fren? fren = null;

            using (IDbConnection db = new SqlConnection(_conStr))
            {
                var parameters = new { 
                    Username = loginRequest.Username, 
                    Password = loginRequest.Password
                };

                fren = await db.QueryFirstOrDefaultAsync<Fren?>("sproc_FrenLogin", parameters, commandType: CommandType.StoredProcedure);
            }

            return fren;
        }
    }
}
