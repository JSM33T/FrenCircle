using FrenCircle.Contracts.Dtos.Requests;
using FrenCircle.Contracts.Dtos.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenCircle.Contracts.Interfaces.Services
{
    public interface IChangeLogRepository
    {
        Task<int> InsertChangeLogAsync(ChangeLogRequestDto dto);
        Task DeleteByVersionAsync(string version);
        public Task<IEnumerable<VersionGroupedChangeLogDto>> GetGroupedByVersionAsync();
    }
}
