using FrenCircle.Contracts.Dtos.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenCircle.Contracts.Interfaces.Services
{
    public interface IProfileService
    {
        Task<UserProfileDetailsDto> GetUserProfileById(int Id);
        Task<int> UpdateUserProfile(EditUserProfileDto userProfileDetails,string AvatarUrl);
    }
}
