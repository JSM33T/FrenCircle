using API.Entities.Shared;
using API.Infra;
using Microsoft.AspNetCore.Http;
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

        protected async Task<IActionResult> ExecuteActionAsync<T>(Func<Task<(int statusCode, T result, string message, List<string> errors)>> action, string methodName)
        {
            var stopwatch = Stopwatch.StartNew();
            var request = _httpContextAccessor.HttpContext.Request;
            var user = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated
                        ? _httpContextAccessor.HttpContext.User.Identity.Name
                        : "Anonymous";

            try
            {
                var (statusCode, result, message, errors) = await action();
                //await Task.Delay(1000);
                return AcResponse(statusCode, message, result, errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in {MethodName}. User: {User}. URL: {Url}. Query: {Query} UserAgent: {UserAgent}", methodName, user, request.Path, request.QueryString, request.Headers.UserAgent);

                return AcResponse(500, "An error occurred while processing your request.", default(T), ["Something went wrong", "Error has been logged"]);
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation("{MethodName} executed in {Duration} ms. User: {User}. URL: {Url}. Query: {Query} UserAgent: {UserAgent}", methodName, stopwatch.ElapsedMilliseconds, user, request.Path, request.QueryString, request.Headers.UserAgent);
            }
        }

        protected IActionResult AcResponse<T>(int status, string message, T data, List<string> hints = null)
        {
            hints ??= [];
            var response = new APIResponse<T>(status, message, data, hints);
            return StatusCode(status, response);
        }
    }
}
