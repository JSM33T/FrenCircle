using System.Security.Cryptography;
using System.Text;
using FrenCircle.Entities.Data;
using FrenCircle.Helpers.Mappers;
using FrenCircle.Helpers.Security;
using FrenCircle.Helpers.SqlQueries;
using FrenCircle.Infra;

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
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>A Task that indicates whether the user's credentials are valid.</returns>
        Task<bool> VerifyUser(VerifyRequest verifyRequest);

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

        Task<bool> IsUserPresentByEmail(string email);

        Task<bool> GenerateAndSaveOTP(string email);
        Task<User?> GetUserByEmail(string email);

    }

    public class AccountRepository(IDapperFactory dapperFactory) : IAccountRepository
    {
        public async Task AddUser(AddUserRequest userRequest)
        {
            var query = DbUsers.Add;

            var user = UserDtoMappers.MAP_AddUserRequest_User(userRequest);

            (user.PasswordHash, user.Salt) = PasswordHasher.HashPassword(userRequest.Password);

            user.Otp = 1111;
            user.OtpTimeStamp = DateTime.Now;

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

        public async Task<bool> GenerateAndSaveOTP(string email)
        {
            // Generate a random 6-digit OTP
            var random = new Random();
            var otp = random.Next(100000, 999999).ToString();

            // Set OTP expiration (e.g., 5 minutes from now)
            var otpDate = DateTime.UtcNow.AddMinutes(30);

            // Save OTP and OTPDate to the database
            var query = "UPDATE Users SET OTP = @OTP, OTPTimeStamp = @OTPDate WHERE Email = @Email";
            var affectedRows = await dapperFactory.Execute(query, new { OTP = otp, OTPDate = otpDate, Email = email });

            return affectedRows > 0;
        }
        
        public async Task<bool> VerifyUser(VerifyRequest verifyRequest)
        {
            var query = "SELECT OTP, OTPTimeStamp FROM Users WHERE Email = @Email";
            var user = await dapperFactory.GetData<User>(query, new { Email = verifyRequest.Email });

            if (user == null || user.Otp != verifyRequest.Otp || user.OtpTimeStamp < DateTime.UtcNow)
            {
                return false;
            }
            var updateQuery = "UPDATE Users SET OTP = NULL, OTPTimeStamp = NULL,IsActive = 1 WHERE Email = @Email";
            await dapperFactory.Execute(updateQuery, new { Email = verifyRequest.Email });

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
    }
}
