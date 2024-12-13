using System.Data;

namespace API.Contracts.Services
{
    public interface IDataService
    {
        /// <summary>
        /// Executes a query and returns multiple rows as a collection of type T.
        /// </summary>
        /// <typeparam name="T">The type to map the rows to.</typeparam>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">Optional parameters to pass to the query.</param>
        /// <returns>A task representing the asynchronous operation, with a collection of type T as the result.</returns>
        Task<IEnumerable<T>> QueryAsync<T>(string query, object parameters = null);

        /// <summary>
        /// Executes a query and returns the first row or default value of type T.
        /// </summary>
        /// <typeparam name="T">The type to map the row to.</typeparam>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="parameters">Optional parameters to pass to the query.</param>
        /// <returns>A task representing the asynchronous operation, with a single mapped object of type T as the result.</returns>
        Task<T> QueryFirstOrDefaultAsync<T>(string query, object parameters = null);

        /// <summary>
        /// Executes a non-query command (e.g., INSERT, UPDATE, DELETE).
        /// </summary>
        /// <param name="query">The SQL command to execute.</param>
        /// <param name="parameters">Optional parameters to pass to the command.</param>
        /// <returns>A task representing the asynchronous operation, with an integer result indicating the number of affected rows.</returns>
        Task<int> ExecuteAsync(string query, object parameters = null);
    }

}
