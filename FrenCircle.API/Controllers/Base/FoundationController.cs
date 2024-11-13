using FrenCircle.Entities.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace FrenCircle.API.Controllers.Base
{
    [ApiController]
    public abstract class FoundationController : ControllerBase
    {
        protected readonly IOptionsMonitor<FCConfig> _config;
        protected readonly ILogger _logger;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly ITelegramService _telegramService;

        public FoundationController(IOptionsMonitor<FCConfig> config, ILogger<FoundationController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _logger = logger;
           // _telegramService = telegramService;
            _httpContextAccessor = httpContextAccessor;
        }

        protected async Task<IActionResult> ExecuteActionAsync<T>(Func<Task<(int statusCode, T result, string message, List<string> errors)>> action)
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
                _logger.LogError(ex, "An error occurred User: {User}. URL: {Url}. Query: {Query} UserAgent: {UserAgent}", user, request.Path, request.QueryString, request.Headers.UserAgent);
                return AcResponse(500, "An error occurred while processing your request.", default(T), ["Something went wrong", "Error has been logged"]);
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation(" executed in {Duration} ms. User: {User}. URL: {Url}. Query: {Query} UserAgent: {UserAgent}", stopwatch.ElapsedMilliseconds, user, request.Path, request.QueryString, request.Headers.UserAgent);
            }
        }

        protected IActionResult AcResponse<T>(int status, string message, T data, List<string>? hints = null)
        {
            var response = new APIResponse<T>(status, message, data, hints);
            return StatusCode(status, response);
        }
    }
}
