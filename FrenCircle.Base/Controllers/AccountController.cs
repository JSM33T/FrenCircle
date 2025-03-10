﻿using FrenCircle.Entities;
using FrenCircle.Entities.Data;
using FrenCircle.Helpers.Security;
using FrenCircle.Helpers.Templates;
using FrenCircle.Infra;
using FrenCircle.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FrenCircle.Base.Controllers
{
    [Route("/api/account")]
    [ApiController]
    public class AccountController(
        IAuthRepository accountRepository,
        IEmailService emailService,
        ITelegramService telegramService,
        ILoginRepository loginRepository,
        INotificationRepository notificationRepository,
        IOptions<FcConfig> config) : FcBaseController
    {
        private readonly FcConfig _config = config.Value;
        private readonly ITelegramService _telegramService = telegramService;
        private readonly ILoginRepository _loginRepository = loginRepository;
        private readonly INotificationRepository _notificationRepository = notificationRepository;


        [HttpPost("create")]
        public async Task<IActionResult> AddUser(AddUserRequest addUserRequest)
        {
            APIResponse<int> apiResponse = new(StatusCodes.Status409Conflict, "Conflict", 0, []);

            if (await accountRepository.IsUserPresentByEmail(addUserRequest.Email))
                apiResponse.Hints.Add("Email is alraedy registered");

            if (await accountRepository.IsUserPresent(addUserRequest.UserName))
                apiResponse.Hints.Add("Username is alraedy registered");

            if (apiResponse.Hints.Count != 0)
                return RESP_Custom(apiResponse);

            await accountRepository.AddUser(addUserRequest);

            _ = _telegramService.SendMessageAsync(
                $"New user registered on {DateTime.UtcNow} \n email: {addUserRequest.Email} \n name:{addUserRequest.FirstName} {addUserRequest.LastName} \n username:{addUserRequest.UserName}");
                
            return RESP_Success("Succssfylly registered");
        }

        [HttpPost("generate-otp")]
        public async Task<IActionResult> GenerateOtp(VerifyRequest verifyRequest)
        {
            if (string.IsNullOrEmpty(verifyRequest.Email))
                return RESP_BadRequestResponse("Email is required.");

            var success = await accountRepository.GenerateAndSaveOtp(verifyRequest.Email);

            if (success != true)
                RESP_NotFoundResponse("User not found.");

            var user = await accountRepository.GetUserByEmail(verifyRequest.Email);

            var body = EmailTemplates.OtpTemplate
                .Replace("{{OTP}}", user!.Otp.ToString())
                .Replace("{{USERNAME}}", user.UserName);

            _ = emailService.SendEmailAsync([user?.Email!], "FrenCircle Account Verification", body);

            return RESP_Success("OTP generated and sent to your email.");
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyUser(VerifyDto verifyRequest)
        {
            if (!await accountRepository.VerifyUser(verifyRequest))
                return RESP_BadRequestResponse("Invalid verification attempt");

            var user = await accountRepository.GetUserByEmail(verifyRequest.Email);

            // Device tracking logic
            var deviceId = Guid.NewGuid();
            var loginInfo = new LoginInfo
            {
                UserId = user!.Id,
                UserAgent = Request.Headers.UserAgent.ToString(),
                DeviceId = deviceId,
                Latitude = decimal.TryParse(Request.Headers["Latitude"], out var latitude) ? latitude : 0,
                Longitude = decimal.TryParse(Request.Headers["Longitude"], out var longitude) ? longitude : 0,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()!,
                LoginMethod = "Verification",
                IsLoggedIn = true
            };
            await _loginRepository.AddLoginEntry(loginInfo);

            var token = JwtTokenHelper.GenerateToken(user,
                _config.JwtSettings?.IssuerSigningKey!,
                _config.JwtSettings!.ValidIssuer,
                _config.JwtSettings.ValidAudience,
                1);

            // Set device identifier cookie
            Response.Cookies.Append("DeviceIdentifier", deviceId.ToString(), new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddYears(1)
            });

            CreateNotificationDto notificationDto = new()
            {
                UserId = user.Id,
                NotificationType = "RecoveryLogin",
                Message = $"You logged in via OTP, please set your password in case you dont remember your old one",
                ActionUrl = "/account/profile/password"
            };
            await _notificationRepository.AddNotification(notificationDto);

            return RESP_Success(new { Token = token });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest loginRequest)
        {
            Guid DeviceIdd = Guid.Empty;
            LoginInfo? loginInfo = null;

            var user = await accountRepository.LoginUser(loginRequest.UserName, loginRequest.Password);
            if (user == null)
                return RESP_BadRequestResponse("Invalid username or password");

            if (!user.IsActive)
                return RESP_BadRequestResponse("Account isn't verified yet. Please verify or recover your account");

            if (Guid.TryParse(Request.Cookies["DeviceIdentifier"], out Guid parsedDeviceId) && parsedDeviceId != Guid.Empty)
            {
                DeviceIdd = (await _loginRepository.GetLoginInfoById(parsedDeviceId)) != null
                    ? parsedDeviceId
                    : Guid.NewGuid();
            }

            var userwithdevicepresent = await _loginRepository.GetLoginInfoByDeviceAndUserId(parsedDeviceId, user.Id);


            if (userwithdevicepresent != null)
            {
                await _loginRepository.ExtendExpiry(parsedDeviceId);
            }
            else
            {
                loginInfo = new LoginInfo
                {
                    UserId = user.Id,
                    UserAgent = Request.Headers["User-Agent"].ToString(),
                    DeviceId = DeviceIdd,
                    Latitude = decimal.TryParse(Request.Headers["Latitude"], out var latitude) ? latitude : 0,
                    Longitude = decimal.TryParse(Request.Headers["Longitude"], out var longitude) ? longitude : 0,
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()!,
                    LoginMethod = "Standard",
                    IsLoggedIn = true
                };
                await _loginRepository.AddLoginEntry(loginInfo);
            }


            if (loginInfo != null)
                if (loginInfo.IsLoggedIn == false)
                    return RESP_BadRequestResponse("You have been logged out login again to continue");
            

            var token = JwtTokenHelper.GenerateToken(user,
                _config.JwtSettings?.IssuerSigningKey!,
                _config.JwtSettings?.ValidIssuer!,
                _config.JwtSettings?.ValidAudience!,
                5);

            var refreshToken = Guid.NewGuid().ToString(); // TODO - accept guid directly and avoid tostring -> conversion

            await accountRepository.StoreRefreshToken(user.Id, refreshToken, DateTime.UtcNow.AddDays(7), DeviceIdd);

            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });


            Response.Cookies.Append("DeviceIdentifier", DeviceIdd.ToString(), new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddYears(1)
            });

            return RESP_Success(new { Token = token });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            if (Guid.TryParse(Request.Cookies["DeviceIdentifier"], out var parsedDeviceId))
            {
                var loginInfo = await _loginRepository.GetLoginInfoById(parsedDeviceId);

                if (loginInfo.IsLoggedIn == false)
                    return RESP_BadRequestResponse("Log in again to continue");
                
            }

            var oldRefreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(oldRefreshToken))
                return BadRequest("No refresh token");

            var storedToken = await accountRepository.GetRefreshToken(oldRefreshToken);

            if (storedToken == null || storedToken.ExpiresAt < DateTime.UtcNow)
                return BadRequest("Invalid or expired refresh token");

            var user = await accountRepository.GetUserById(storedToken.UserId);

            if (user == null)
                return BadRequest("User not found");

            var newAccessToken = JwtTokenHelper.GenerateToken(user,
                _config.JwtSettings?.IssuerSigningKey!,
                _config.JwtSettings?.ValidIssuer!,
                _config.JwtSettings?.ValidAudience!,
                5);

            var newRefreshToken = Guid.NewGuid().ToString();

            await accountRepository.UpdateRefreshToken(storedToken.UserId, oldRefreshToken, newRefreshToken, DateTime.UtcNow.AddDays(7));

            Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return RESP_Success(new { Token = newAccessToken }, "No refresh token");
        }

        [HttpPost("outlook")]
        public async Task<IActionResult> OutlookMessage(OutlookMessage message)
        {
            _ =  _telegramService.SendMessageAsync(
               $"Outlook Message: \n {message.Message}");

            return RESP_Success("OTP generated and sent to your email.");
        }


    }

    public class OutlookMessage
    {
        public string Message { get; set; }
    }
}