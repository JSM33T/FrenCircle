using FrenCircle.Contracts.Dtos.Requests;
using FrenCircle.Contracts.Dtos.Responses;
using FrenCircle.Contracts.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FrenCircle.Api.Controllers
{
    [Route("api/changelog")]
    [ApiController]
    public class ChangeLogController : FcBaseController
    {
        private readonly IChangeLogService _changeLogService;
        public ChangeLogController(IChangeLogService changeLogService)
        {
            _changeLogService = changeLogService;
        }
        [HttpPost("upsert")]
        public async Task<IActionResult> UpsertChangeLog([FromBody] ChangeLogBulkRequestDto dto)
        {
            var id = await _changeLogService.AddBulkChangeLogsAsync(dto);
            return Ok(new { Message = "ChangeLog saved", Id = id });
        }

        [HttpGet("grouped")]
        public async Task<IActionResult> GetGroupedByVersion()
        {
            var result = await _changeLogService.GetGroupedByVersionAsync();
            return Ok(result);
        }


    }
}
