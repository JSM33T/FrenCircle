using API.Entities.Dedicated;

namespace API.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves user details by ID.
        /// </summary>
        Task<Member> GetUserDetailsByIdAsync(int userId);

        /// <summary>
        /// Gets a user if present based on a specific property and value.
        /// </summary>
        Task<Member> GetUserByProp(string propertyName, object value);

        /// <summary>
        /// Adds a new user to the repository.
        /// </summary>
        /// <param name="member">The member entity containing the user details to be added.</param>
        Task<Member> AddUserAsync(Member member);


        Task UpdateTimeSpent(int Id);
    }
}
