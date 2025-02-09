using FrenCircle.Entities;
using FrenCircle.Entities.Data;
using FrenCircle.Repositories;
using Microsoft.AspNetCore.Mvc;
using FrenCircle.Helpers.Security;
using FrenCircle.Helpers.Templates;
using FrenCircle.Infra;
using Microsoft.Extensions.Options;

namespace FrenCircle.Base.Controllers
{
    [Route("/api/account")]
    [ApiController]
    public class AccountController(
        IAccountRepository accountRepository,
        IEmailService emailService,
        ITelegramService telegramService,
        IOptions<FcConfig> config) : FcBaseController
    {
        private readonly FcConfig _config = config.Value;
        private readonly ITelegramService _telegramService = telegramService;

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

            _ = _telegramService.SendMessageAsync($"New user registered on {DateTime.Now} \n email: {addUserRequest.Email} \n name:{addUserRequest.FirstName} {addUserRequest.LastName} \n username:{addUserRequest.UserName}");

            await accountRepository.AddUser(addUserRequest);

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

            await emailService.SendEmailAsync([user?.Email!], "FrenCircle Account Verification", body);

            return RESP_Success("OTP generated and sent to your email.");
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyUser(VerifyDto verifyRequest)
        {
            if (!await accountRepository.VerifyUser(verifyRequest))
                return RESP_BadRequestResponse("Invalid verification attempt");

            var user = await accountRepository.GetUserByEmail(verifyRequest.Email);

            var token = JwtTokenHelper.GenerateToken(user!,
                _config.JwtSettings?.IssuerSigningKey!,
                _config.JwtSettings!.ValidIssuer,
                _config.JwtSettings.ValidAudience,
                30000);

            return RESP_Success(new { Token = token });
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login(LoginUserRequest loginRequest)
        //{
        //    var user = await accountRepository.LoginUser(loginRequest.UserName, loginRequest.Password);

        //    if (user == null)
        //        return RESP_BadRequestResponse("Invalid username or password");

        //    if (user.IsActive == false)
        //        return RESP_BadRequestResponse("Account isn't verified yet. Please verify or recover your account");

        //    var token = JwtTokenHelper.GenerateToken(user,
        //        _config.JwtSettings?.IssuerSigningKey!,
        //        _config.JwtSettings?.ValidIssuer!,
        //        _config.JwtSettings?.ValidAudience!,
        //        30000);

        //    // Return the token
        //    return RESP_Success(new { Token = token });
        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest loginRequest)
        {
            var user = await accountRepository.LoginUser(loginRequest.UserName, loginRequest.Password);
            if (user == null)
                return RESP_BadRequestResponse("Invalid username or password");

            if (!user.IsActive)
                return RESP_BadRequestResponse("Account isn't verified yet. Please verify or recover your account");

            var token = JwtTokenHelper.GenerateToken(user,
                _config.JwtSettings.IssuerSigningKey!,
                _config.JwtSettings.ValidIssuer!,
                _config.JwtSettings.ValidAudience!,
                1); // 10 min expiry

            var refreshToken = Guid.NewGuid().ToString();
            await accountRepository.StoreRefreshToken(user.Id, refreshToken, DateTime.UtcNow.AddDays(1));

            // Store refresh token in HttpOnly cookie
            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(1)
            });

            return RESP_Success(new { Token = token });
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var oldRefreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(oldRefreshToken))
                return Unauthorized("No refresh token");

            var storedToken = await accountRepository.GetRefreshToken(oldRefreshToken);
            if (storedToken == null || storedToken.ExpiresAt < DateTime.UtcNow)
                return Unauthorized("Invalid or expired refresh token");

            var user = await accountRepository.GetUserById(storedToken.UserId);
            if (user == null)
                return Unauthorized("User not found");

            var newAccessToken = JwtTokenHelper.GenerateToken(user,
                _config.JwtSettings.IssuerSigningKey!,
                _config.JwtSettings.ValidIssuer!,
                _config.JwtSettings.ValidAudience!,
                1); // 10 min expiry

            var newRefreshToken = Guid.NewGuid().ToString();
            await accountRepository.UpdateRefreshToken(storedToken.UserId, oldRefreshToken, newRefreshToken, DateTime.UtcNow.AddDays(1));

            // Set new refresh token in cookie
            Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(1)
            });

            return Ok(new { Token = newAccessToken });
        }

    }
}