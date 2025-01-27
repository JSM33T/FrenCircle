using FrenCircle.Entities.Data;

namespace FrenCircle.Repositories
{
    /// <summary>
    /// Interface for an account repository to manage user accounts.
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Adds a new user to the repository.
        /// </summary>
        /// <param name="user">The user to add.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task AddUser(User user);

        /// <summary>
        /// Logs in a user with the provided username and password.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>A Task that may return the logged-in user or null.</returns>
        Task<User?> LoginUser(string username, string password);

        /// <summary>
        /// Verifies the credentials of a user.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>A Task that indicates whether the user's credentials are valid.</returns>
        Task<bool> VerifyUser(string username, string password);

        /// <summary>
        /// Retrieves a list of all users from the repository.
        /// </summary>
        /// <returns>A Task that returns a list of all users.</returns>
        Task<List<User>> GetAllUsers();

        /// <summary>
        /// Checks if a user with the specified username is present in the repository.
        /// </summary>
        /// <param name="username">The username to check.</param>
        /// <returns>A Task that indicates whether the user is present in the repository.</returns>
        Task<bool> IsUserPresent(string username);
    }
    
    public class AccountRepository : IAccountRepository
    {

        public Task AddUser(User user)
        {
            throw new NotImplementedException();
        }
        public Task<User?> LoginUser(string username, string password)
        {
            throw new NotImplementedException();
        }
        public Task<bool> VerifyUser(string username, string password)
        {
            throw new NotImplementedException();
        }
        public Task<List<User>> GetAllUsers()
        {
            throw new NotImplementedException();
        }
        public Task<bool> IsUserPresent(string username)
        {
            throw new NotImplementedException();
        }
    }
}
