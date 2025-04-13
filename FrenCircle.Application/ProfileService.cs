using FrenCircle.Contracts.Dtos.Responses;
using FrenCircle.Contracts.Interfaces.Repositories;
using FrenCircle.Contracts.Interfaces.Services;

namespace FrenCircle.Application
{
    public class ProfileService(IProfileRepository profileRepository) : IProfileService
    {
        public async Task<UserProfileDetailsDto> GetUserProfileById(int Id)
            => await profileRepository.GetUserProfileById(Id);
    }
}
