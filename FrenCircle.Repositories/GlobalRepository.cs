
using Dapper;
using FrenCircle.Entities.Shared;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;

namespace FrenCircle.Repositories
{
    public class GlobalRepository : IGlobalRepository
    {

        protected readonly IOptionsMonitor<FCConfig> _config;
        private readonly IDbConnection _dbConnection;
        protected readonly ILogger _logger;
        private string _conStr;

        public GlobalRepository(IOptionsMonitor<FCConfig> config, ILogger<GlobalRepository> logger, IDbConnection dbConnection)
        {
            _config = config;
            _logger = logger;
            _conStr = _config.CurrentValue.ConnectionString;
            _dbConnection = dbConnection;
        }

        public async Task<string> GetGLobalValue(string Key)
        {
            using IDbConnection db = new SqlConnection(_conStr);

            string? Val = await db.QueryFirstAsync<string>("SELECT [Value] FROM tblGlobalSettings WITH(NOLOCK) WHERE [Key] = 'default'");

            return  Val;
        }
    }
}
