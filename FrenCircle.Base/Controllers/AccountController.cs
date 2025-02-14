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
        APIResponse<int> apiResponse = new(StatusCodes.Status200OK, "Success", 0, []);
        
        var aa = await _accountRepository.GetProfileInfo(_claimsService.GetUserId(User));

        var request = new GetProfileRequest()
        {
            FirstName = aa?.FirstName,
            LastName = aa.LastName,
            UserName = aa.UserName,
            Bio = aa.Bio,
            Email = aa.Email
            
        };
        
        return RESP_Success(request,"Successfully registered");
    }
}