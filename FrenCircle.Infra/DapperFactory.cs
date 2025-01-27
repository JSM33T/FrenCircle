using Dapper;
using Microsoft.Extensions.Options;
using Microsoft.Data.SqlClient;
using FrenCircle.Entities;

namespace FrenCircle.Infra
{
    /// <summary>
    /// Interface for a Dapper factory to retrieve data asynchronously.
    /// </summary>
    public interface IDapperFactory
    {
        /// <summary>
        /// Retrieves a single data record asynchronously based on the provided query and parameters.
        /// </summary>
        /// <typeparam name="T">The type of the data record to retrieve.</typeparam>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">Optional parameters for the query.</param>
        /// <returns>A Task representing the asynchronous operation that returns the data record.</returns>
        public Task<T?> GetData<T>(string query, object? parameters = null);

        /// <summary>
        /// Retrieves a list of data records asynchronously based on the provided query and parameters.
        /// </summary>
        /// <typeparam name="T">The type of the data records to retrieve.</typeparam>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">Optional parameters for the query.</param>
        /// <returns>A Task representing the asynchronous operation that returns a list of data records.</returns>
        public Task<IEnumerable<T>> GetDataList<T>(string query, object? parameters = null);
    }
    public class DapperFactory(IOptions<FCConfig> config) : IDapperFactory
    {
        private readonly string _connectionString = config.Value.ConnectionString;
        
        public async Task<T?> GetData<T>(string query, object? parameters = null)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<T>(query, parameters);
        }
        
        public async Task<IEnumerable<T>> GetDataList<T>(string query, object? parameters = null)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.QueryAsync<T>(query, parameters);
        }
    }
}
