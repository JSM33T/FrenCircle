﻿using System.Security.Cryptography;
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
    /// Interface for an account repository to manage user accounts.
    /// </summary>
    public interface IAuthRepository
    {
        /// <summary>
        /// Adds a new user to the repository.
        /// </summary>
        /// <param name="userRequest">The user to add.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task AddUser(AddUserRequest userRequest);

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
        /// <param name="verifyRequest">The username and password object of the user.</param>
        /// <returns>A Task that indicates whether the user's credentials are valid.</returns>
        Task<bool> VerifyUser(VerifyDto verifyRequest);

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
        /// <summary>
        /// Checks if a user with the specified email is present in the repository.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>A Task that indicates whether the user is present in the repository.</returns>
        Task<bool> IsUserPresentByEmail(string email);

        /// <summary>
        /// Generates and saves a one-time password (OTP) for the user with the specified email.
        /// </summary>
        /// <param name="email">The email of the user to generate the OTP for.</param>
        /// <returns>A Task that indicates whether the OTP was successfully generated and saved.</returns>
        Task<bool> GenerateAndSaveOtp(string email);

        /// <summary>
        /// Retrieves a user by their email.
        /// </summary>
        /// <param name="email">The email of the user to retrieve.</param>
        /// <returns>A Task that may return the user or null if not found.</returns>
        Task<User?> GetUserByEmail(string email);

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="Id">The ID of the user to retrieve.</param>
        /// <returns>A Task that may return the user or null if not found.</returns>
        Task<User?> GetUserById(int Id);

        /// <summary>
        /// Stores a refresh token for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="refreshToken">The refresh token to store.</param>
        /// <param name="expiryDate">The expiry date of the refresh token.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task StoreRefreshToken(int userId, string refreshToken, DateTime expiryDate,Guid deviceId);

        /// <summary>
        /// Retrieves a refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token to retrieve.</param>
        /// <returns>A Task that may return the refresh token or null if not found.</returns>
        Task<RefreshToken?> GetRefreshToken(string refreshToken);

        /// <summary>
        /// Updates a refresh token for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="oldRefreshToken">The old refresh token to update.</param>
        /// <param name="newRefreshToken">The new refresh token to store.</param>
        /// <param name="expiryDate">The expiry date of the new refresh token.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task UpdateRefreshToken(int userId, string oldRefreshToken, string newRefreshToken, DateTime expiryDate);
    }

    public class AuthRepository(IDapperFactory dapperFactory) : IAuthRepository
    {
        public async Task AddUser(AddUserRequest userRequest)
        {
            var query = DbUsers.Add;

            var user = UserDtoMappers.MAP_AddUserRequest_User(userRequest);

            (user.PasswordHash, user.Salt) = PasswordHasher.HashPassword(userRequest.Password);
            var random = new Random();
            user.Otp = random.Next(1000, 9999);
            user.OtpTimeStamp = DateTime.UtcNow;

            var id = await dapperFactory.GetData<int>(query, new
            {
                user.FirstName,
                user.LastName,
                user.UserName,
                user.Email,
                user.Bio,
                user.PasswordHash,
                user.Salt,
                user.TimeSpent,
                user.DateUpdated,
                user.LastSeen,
                user.DateAdded,
                user.Otp,
                user.OtpTimeStamp
            });

            user.Id = id;
        }

        public async Task<User?> LoginUser(string username, string password)
        {
            var query = DbUsers.Login;
            var user = await dapperFactory.GetData<User>(query, new { Username = username });

            if (user == null || !PasswordHasher.VerifyPassword(password, user.PasswordHash, user.Salt))
                return null;

            return user;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            var query = DbUsers.GetByEmail;
            var user = await dapperFactory.GetData<User>(query, new { Email = email });
            return user;
        }

        public async Task<User?> GetUserById(int Id)
        {
            var query = DbUsers.GetById;
            var user = await dapperFactory.GetData<User>(query, new { Id = Id });
            return user;
        }

        public async Task<bool> GenerateAndSaveOtp(string email)
        {
            var random = new Random();
            var otp = random.Next(1000, 9999);

            var otpDate = DateTime.UtcNow.AddMinutes(30);

            var query = DbUsers.GenerateOtp;
            var affectedRows = await dapperFactory.Execute(query, new { OTP = otp, OTPDate = otpDate, Email = email });

            return affectedRows > 0;
        }

        public async Task<bool> VerifyUser(VerifyDto verifyRequest)
        {
            var query = DbUsers.GetOtp;
            var user = await dapperFactory.GetData<User>(query, new { verifyRequest.Email });

            if (user == null || user.Otp != verifyRequest.Otp || user.OtpTimeStamp < DateTime.UtcNow)
            {
                return false;
            }
            var updateQuery = DbUsers.VerifyOtp;
            await dapperFactory.Execute(updateQuery, new { verifyRequest.Email });

            return true;
        }

        public async Task<List<User>> GetAllUsers()
        {
            var query = DbUsers.GetAll;
            var users = await dapperFactory.GetDataList<User>(query);
            return users.ToList();
        }

        public async Task<bool> IsUserPresent(string username)
        {
            var query = DbUsers.CheckByUsername;
            var user = await dapperFactory.GetData<User>(query, new { Username = username });
            return user != null;
        }

        public async Task<bool> IsUserPresentByEmail(string email)
        {
            var query = DbUsers.CheckByEmail;
            var user = await dapperFactory.GetData<User>(query, new { Email = email });
            return user != null;
        }

        // 🔹 Store Refresh Token
        public async Task StoreRefreshToken(int userId, string refreshToken, DateTime expiryDate,Guid deviceId)
        {
            var query = DbUsers.StoreRefreshToken;
            await dapperFactory.Execute(query, new { UserId = userId, RefreshToken = refreshToken, ExpiryDate = expiryDate,DeviceId = deviceId });
        }

        // 🔹 Get Refresh Token
        public async Task<RefreshToken?> GetRefreshToken(string refreshToken)
        {
            var query = DbUsers.GetRefreshToken;
            return await dapperFactory.GetData<RefreshToken>(query, new { RefreshToken = refreshToken });
        }

        // 🔹 Update Refresh Token (Rotating Tokens)
        public async Task UpdateRefreshToken(int userId, string oldRefreshToken, string newRefreshToken, DateTime expiryDate)
        {
            var query = DbUsers.UpdateRefreshToken;
            await dapperFactory.Execute(query, new { UserId = userId, OldRefreshToken = oldRefreshToken, NewRefreshToken = newRefreshToken, ExpiryDate = expiryDate });
        }
    }
}
