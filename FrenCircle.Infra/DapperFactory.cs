using Dapper;
using Microsoft.Extensions.Options;
using Microsoft.Data.SqlClient;
using FrenCircle.Entities;

namespace FrenCircle.Infra
{
    public interface IDapperFactory
    {
        public Task<T> GetData<T>(string query, object parameters = null);

        public Task<IEnumerable<T>> GetDataList<T>(string query, object parameters = null);

    }
    public class DapperFactory : IDapperFactory
    {

        private readonly string _connectionString;

        public DapperFactory(IOptions<FCConfig> config)
        {
            _connectionString = config.Value.ConnectionString;
        }

        // Change this method to return a Task<T> since it's being awaited
        public async Task<T> GetData<T>(string query, object parameters = null)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<T>(query, parameters);
        }

        // This method is already returning IEnumerable, so it will work fine
        public async Task<IEnumerable<T>> GetDataList<T>(string query, object parameters = null)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.QueryAsync<T>(query, parameters);
        }
    }

}
