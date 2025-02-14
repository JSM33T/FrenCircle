using System.Security.Cryptography;
using System.Text;
using FrenCircle.Entities.Data;
using FrenCircle.Entities.Shared;
using FrenCircle.Helpers.Mappers;
using FrenCircle.Helpers.Security;
using FrenCircle.Helpers.SqlQueries;
using FrenCircle.Infra;

namespace FrenCircle.Repositories
{
    /// <summary>
    /// Interface for an account repository to manage user profiles.
    /// </summary>
    public interface IAccountRepository
    {
        /// <summary>
        /// Retrieves the profile information of a user by their ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A Task that returns the user's profile information.</returns>
        Task<User?> GetProfileInfo(int userId);

        /// <summary>
        /// Updates the profile information of a user.
        /// </summary>
        /// <param name="updateRequest">The profile update request containing new details.</param>
        /// <returns>A Task that indicates whether the update was successful.</returns>
        Task<bool> EditProfile(EditProfileRequest updateRequest);
    }

    public class AccountRepository(IDapperFactory dapperFactory) : IAccountRepository
    {
        public async Task<User?> GetProfileInfo(int userId)
        {
            var query = DbUsers.GetProfile;
            var profile = await dapperFactory.GetData<User>(query, new { UserId = userId });
            return profile;
        }

        public async Task<bool> EditProfile(EditProfileRequest updateRequest)
        {
            var query = DbUsers.EditProfile;
            var affectedRows = await dapperFactory.Execute(query, new
            {
                updateRequest.FirstName,
                updateRequest.LastName,
                updateRequest.Bio,
                updateRequest.UserName
            });
            return affectedRows > 0;
        }
    }
}