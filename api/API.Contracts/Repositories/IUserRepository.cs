using API.Entities.Dedicated;

namespace API.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves user details by ID.
        /// </summary>
        Task<Fren> GetUserDetailsByIdAsync(int userId);

        /// <summary>
        /// Gets a user if present based on a specific property and value.
        /// </summary>
        Task<Fren> GetUserByProp(string propertyName, object value);

        /// <summary>
        /// Adds a new user to the repository.
        /// </summary>
        /// <param name="member">The member entity containing the user details to be added.</param>
        Task<Fren> AddUserAsync(Fren member);
        
        /// <summary>
        /// Adds a new user to the repository.
        /// </summary>
        /// <param name="id">The member id of the active user present.</param>
        Task UpdateTimeSpent(int Id);
    }
}
