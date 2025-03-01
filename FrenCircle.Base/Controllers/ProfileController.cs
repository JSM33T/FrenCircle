using FrenCircle.Entities;
using FrenCircle.Entities.Data;
using FrenCircle.Infra;
using FrenCircle.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
namespace FrenCircle.Base.Controllers;

[Route("/api/profile")]
[ApiController]
[Authorize]
public class ProfileController(IOptions<FcConfig> config,IAccountRepository accountRepository,IClaimsService claimsService) : FcBaseController
{
    private readonly FcConfig _config = config.Value;
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly IClaimsService _claimsService = claimsService;
    
    [HttpGet("get")]
    public async Task<IActionResult> GetProfileDetails()
    {
        //_ = new APIResponse<int>(StatusCodes.Status200OK, "Success", 0, []);

        var userDetails = await _accountRepository.GetProfileInfo(_claimsService.GetUserId(User));

        var response = new GetProfileResponse()
        {
            FirstName = userDetails!.FirstName,
            LastName = userDetails!.LastName,
            UserName = userDetails!.UserName,
            Bio = userDetails.Bio,
            Email = userDetails.Email,
            DateAdded = userDetails.DateAdded,
            DateUpdated = userDetails.DateUpdated,
            TimeSpent = userDetails.TimeSpent
        };
        
        return RESP_Success(response, "Profile Details Retrieved");
    }

    [HttpGet("getlogins")]
    public async Task<IActionResult> GetLoginEntries()
    {
        //_ = new APIResponse<int>(StatusCodes.Status200OK, "Success", 0, []);

        var userDetails = await _accountRepository.GetLoginInfo(_claimsService.GetUserId(User));

        return RESP_Success(userDetails, "Profile Details Retrieved");
    }
}