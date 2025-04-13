using FrenCircle.Contracts.Dtos.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenCircle.Contracts.Interfaces.Repositories
{
    public interface IProfileRepository
    {
        Task<UserProfileDetailsDto> GetUserProfileById(int Id);
    }
}
