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
public class AccountController(IOptions<FcConfig> config,IAccountRepository accountRepository,IClaimsService claimsService) : FcBaseController
{
    private readonly FcConfig _config = config.Value;
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly IClaimsService _claimsService = claimsService;
    
    [HttpGet("get")]
    public async Task<IActionResult> AddUser()
    {
        _ = new APIResponse<int>(StatusCodes.Status200OK, "Success", 0, []);

        var userDetails = await _accountRepository.GetProfileInfo(_claimsService.GetUserId(User));

        var request = new GetProfileRequest()
        {
            FirstName = userDetails!.FirstName,
            LastName = userDetails!.LastName,
            UserName = userDetails!.UserName,
            Bio = userDetails.Bio,
            Email = userDetails.Email
        };
        
        return RESP_Success(request,"Successfully registered");
    }
}