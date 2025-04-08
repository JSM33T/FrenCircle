using FrenCircle.Contracts.Dtos;
using FrenCircle.Contracts.Dtos.Requests;
using FrenCircle.Contracts.Dtos.Responses;
using FrenCircle.Contracts.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FrenCircle.Api.Controllers
{
    [Route("api/changelog")]
    [ApiController]
    public class ChangeLogController(IChangeLogService changeLogService) : FcBaseController
    {
        [HttpPost("upsert")]
        public async Task<ActionResult<ApiResponse<object>>> UpsertChangeLog([FromBody] ChangeLogBulkRequestDto dto)
        {
            var id = await changeLogService.AddBulkChangeLogsAsync(dto);

            return RESP_Success<object>(new { Id = id }, "Changelog saved successfully!");

        }

        [HttpGet("grouped")]
        public async Task<ActionResult<ApiResponse<IEnumerable<VersionGroupedChangeLogDto>>>> GetGroupedByVersion()
             => RESP_Success(await changeLogService.GetGroupedByVersionAsync());
    }
}
