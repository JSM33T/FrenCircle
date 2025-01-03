using API.Contracts.Services;
using API.Entities.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace API.Base.Controllers
{
    [ApiController]
    public abstract class FoundationController : ControllerBase
    {
        protected readonly IOptionsMonitor<Jsm33tConfig> _config;
        protected readonly ILogger _logger;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommonService _commonService;

        public FoundationController(IOptionsMonitor<Jsm33tConfig> config, ILogger<FoundationController> logger, IHttpContextAccessor httpContextAccessor, ICommonService commonService)
        {
            _config = config;
            _logger = logger;
            _commonService = commonService;
            _httpContextAccessor = httpContextAccessor;
        }

        protected async Task<IActionResult> ExecuteActionAsync<T>(Func<Task<APIResponse<T>>> action, string methodName)
        {
            var stopwatch = Stopwatch.StartNew();
            var request = _httpContextAccessor.HttpContext.Request;
            var user = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated
                        ? _httpContextAccessor.HttpContext.User.Identity.Name
                        : "Anonymous";
            var apiResponse = await action();
            try
            {
                
                //await Task.Delay(1000);
                return AcResponse(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in {MethodName}. User: {User}. URL: {Url}. Query: {Query} UserAgent: {UserAgent}", methodName, user, request.Path, request.QueryString, request.Headers.UserAgent);


                apiResponse = new APIResponse<T>(100, "An error occurred while processing your request.", default(T), []);
               
                return AcResponse(apiResponse);
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation("{MethodName} executed in {Duration} ms. User: {User}. URL: {Url}. Query: {Query} UserAgent: {UserAgent}", methodName, stopwatch.ElapsedMilliseconds, user, request.Path, request.QueryString, request.Headers.UserAgent);
            }
        }

        protected IActionResult AcResponse<T>(APIResponse<T> apiResponse)
        {
            return StatusCode(apiResponse.Status,apiResponse);
        }
    }
}
