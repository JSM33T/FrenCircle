
using Dapper;
using FrenCircle.Repositories.Database;
using Microsoft.Extensions.Logging;
using System.Data;

namespace FrenCircle.Repositories
{
    public class GlobalRepository(ILogger<GlobalRepository> logger, IDbConnectionFactory dbConnectionFactory) : IGlobalRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;
        protected readonly ILogger _logger = logger;

        public async Task<string> GetGlobalValue(string key)
        {
            using IDbConnection db = await _dbConnectionFactory.CreateConnectionAsync();

            var val = await db.QueryFirstOrDefaultAsync<string>("SELECT [Value] FROM tblGlobalSettings WITH(NOLOCK) WHERE [Key] = @Key", new { Key = key });

            return val ?? string.Empty;

        }
    }
}
