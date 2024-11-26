using Microsoft.Data.SqlClient;
using System.Data;

namespace FrenCircle.Repositories.Database
{
    public class MsSqlDbConnectionFactory : IDbConnectionFactory
    {
        private string _connectionString;
        public MsSqlDbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(token);
            return connection;
        }
    }
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default);
    }
}
