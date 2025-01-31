using FrenCircle.Entities;
using FrenCircle.Entities.Data;
using FrenCircle.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FrenCircle.Helpers.Security;

namespace FrenCircle.Base.Controllers
{
    [Route("/api/account")]
    [ApiController]
    public class AccountController(IAccountRepository accountRepository) : FcBaseController
    {
        [HttpPost("create")]
        public async Task<IActionResult> AddUser(AddUserRequest addUserRequest)
        {
            APIResponse<int> aPIResponse = new(StatusCodes.Status409Conflict, "Conflict", 0, []);

            if (await accountRepository.IsUserPresentByEmail(addUserRequest.Email))
                aPIResponse.Hints.Add("Email is alraedy registered");

            if (await accountRepository.IsUserPresent(addUserRequest.UserName))
                aPIResponse.Hints.Add("Username is alraedy registered");

            if (aPIResponse.Hints.Count != 0)
                return RESP_Custom(aPIResponse);

            await accountRepository.AddUser(addUserRequest);

            return RESP_Success("Succssfylly registered");
        }
        
        [HttpPost("generate-otp")]
        public async Task<IActionResult> GenerateOTP(VerifyRequest verifyRequest)
        {
            if (string.IsNullOrEmpty(verifyRequest.Email))
                return RESP_BadRequestResponse("Email is required.");

            var success = await accountRepository.GenerateAndSaveOTP(verifyRequest.Email);

            if (!success)
                return RESP_NotFoundResponse("User not found.");

            // In a real-world scenario, send the OTP to the user's email
            return RESP_Success("OTP generated and sent to your email.");
        }
        
        [HttpPost("verify")]
        public async Task<IActionResult> VerifyUser(VerifyDto verifyRequest)
        {
            if (!await accountRepository.VerifyUser(verifyRequest))
                return RESP_BadRequestResponse("Invalid verification attempt");
            
            var user = await accountRepository.GetUserByEmail(verifyRequest.Email);
            
            var token = JwtTokenHelper.GenerateToken(user, 
                "iureowtueorituowierutoi4354======",
                "www.frencircle.com", 
                "www.frencircle.com",
                3000);
            
            return RESP_Success(new { Token = token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserRequest loginRequest)
        {
            var user = await accountRepository.LoginUser(loginRequest.UserName, loginRequest.Password);

            if (user == null)
                return RESP_BadRequestResponse("Invalid username or password");

            if (user.IsActive == false)
                return RESP_BadRequestResponse("Account isn't verified yet. Please verify or recover your account");
            
            var token = JwtTokenHelper.GenerateToken(user, 
                "iureowtueorituowierutoi4354======",
                "www.frencircle.com", 
                "www.frencircle.com",
                3000);

            // Return the token
            return RESP_Success(new { Token = token });
        }
    }
}