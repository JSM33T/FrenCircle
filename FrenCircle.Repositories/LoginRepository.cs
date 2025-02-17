using FrenCircle.Entities.Data;
using FrenCircle.Infra;
using FrenCircle.Helpers.SqlQueries;

namespace FrenCircle.Repositories
{
    /// <summary>
    /// Interface for managing login information.
    /// </summary>
    public interface ILoginRepository
    {
        /// <summary>
        /// Retrieves login information by DeviceId.
        /// </summary>
        /// <param name="deviceId">The device identifier.</param>
        /// <returns>A Task that returns the login information.</returns>
        Task<LoginInfo> GetLoginInfoById(Guid deviceId);
        
        Task<LoginInfo> GetLoginInfoByDeviceAndUserId(Guid deviceId,int UserId);

        /// <summary>
        /// Adds a new login entry.
        /// </summary>
        /// <param name="loginInfo">The login information to add.</param>
        /// <returns>A Task that returns the added login information.</returns>
        Task<LoginInfo> AddLoginEntry(LoginInfo loginInfo);

        /// <summary>
        /// Extends the expiry of the login session.
        /// </summary>
        /// <returns>A Task that indicates whether the expiry was extended.</returns>
        Task<bool> ExtendExpiry(Guid deviceId);
    }

    /// <summary>
    /// Repository implementation for managing login information.
    /// </summary>
    public class LoginRepository(IDapperFactory dapperFactory,IClaimsService claimsService) : ILoginRepository
    {
        private readonly IClaimsService _claimsService = claimsService;
        public async Task<LoginInfo> GetLoginInfoById(Guid deviceId)
        {
            var query = DbLogin.GetLoginByDeviceId;
            
            var loginInfo = await dapperFactory.GetData<LoginInfo>(query, new { DeviceId = deviceId });
            return loginInfo;
        }
        
        public async Task<LoginInfo> GetLoginInfoByDeviceAndUserId(Guid deviceId,int UserId)
        {
            var query = DbLogin.GetLoginByDeviceAndUserId;
            
            var loginInfo = await dapperFactory.GetData<LoginInfo>(query, new { DeviceId = deviceId,UserId = UserId });
            return loginInfo;
        }
        

        public async Task<LoginInfo> AddLoginEntry(LoginInfo loginInfo)
        {
            var query = DbLogin.AddLoginEntry;

            var id = await dapperFactory.GetData<int>(query, new
            {
                loginInfo.UserId,
                loginInfo.UserAgent,
                loginInfo.DeviceId,
                loginInfo.Latitude,
                loginInfo.Longitude,
                loginInfo.IpAddress,
                loginInfo.LoginMethod,
                loginInfo.IsLoggedIn
            });

            loginInfo.Id = id;
            return loginInfo;
        }

        public async Task<bool> ExtendExpiry(Guid deviceId)
        {
            var query = DbLogin.ExtendExpiry;
            var affectedRows = await dapperFactory.Execute(query, new { DeviceId = deviceId });
            return affectedRows > 0;
        }
    }
}
