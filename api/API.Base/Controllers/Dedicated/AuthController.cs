//using API.Contracts.Services;
//using API.Entities.Dedicated.Auth;
//using API.Entities.Shared;
//using Google.Apis.Auth;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using static Google.Apis.Auth.GoogleJsonWebSignature;

//namespace API.Base.Controllers.Dedicated
//{
//    public class GoogleLoginRequest
//    {
//        public string IdToken { get; set; }
//    }

//    [Route("api/auth")]
//    [ApiController]
//    public class AuthController(IOptionsMonitor<Jsm33tConfig> config, ILogger<FoundationController> logger, IHttpContextAccessor httpContextAccessor, ICommonService commonService) : FoundationController(config, logger, httpContextAccessor, commonService)
//    {
//        private readonly ICommonService _commonService = commonService;

//        [HttpPost("google-login")]
//        [AllowAnonymous]
//        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
//        {
//            return await ExecuteActionAsync(async () =>
//            {
//                APIResponse<AddUserRequst> apiResponse = new(StatusCodes.Status404NotFound, "Init google login/signup", null, []);


//                var settings = new GoogleJsonWebSignature.ValidationSettings()
//                {
//                    Audience = [config.CurrentValue.logins.GoogleClientId]
//                };

//                Payload payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);

//                apiResponse.Data = new AddUserRequst()
//                {
//                    FirstName = payload.Name,
//                    LastName = payload.Name,
//                    IsActive = true,
//                    Username = payload.GivenName
//                };

//                return (apiResponse);

//            }, "Contact method");
//        }

//    }
//}
