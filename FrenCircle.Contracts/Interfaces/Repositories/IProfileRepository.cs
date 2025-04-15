using FrenCircle.Contracts.Dtos.Responses;

namespace FrenCircle.Contracts.Interfaces.Repositories
{
    public interface IProfileRepository
    {
        Task<UserProfileDetailsDto> GetUserProfileById(int Id);
        Task<int> UpdateUserProfile(EditUserProfileDto userProfileDetailsDto,string AvatarUrl);
    }
}
