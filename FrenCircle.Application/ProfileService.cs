using FrenCircle.Contracts.Dtos.Responses;
using FrenCircle.Contracts.Interfaces.Repositories;
using FrenCircle.Contracts.Interfaces.Services;

namespace FrenCircle.Application
{
    public class ProfileService(IProfileRepository profileRepository) : IProfileService
    { 
        //public async Task<UserProfileDetailsDto> GetUserProfileById(int Id)
        //{
        //    var ss = await profileRepository.GetUserProfileById(Id);          
        //    return ss;
        //}

        public async Task<UserProfileDetailsDto> GetUserProfileById(int id)
        {
            var profile = await profileRepository.GetUserProfileById(id);

            if (!string.IsNullOrEmpty(profile.Avatar))
            {
                // Replace "https://res.cloudinary.com/" with your app's media cache endpoint
                profile.Avatar = profile.Avatar.Replace(
                    "https://res.cloudinary.com/",
                    "http://localhost:5035/media/"
                );
            }

            return profile;
        }

        public async Task<int> UpdateUserProfile(EditUserProfileDto userProfileDetails, string AvatarUrl)
        {
            return await profileRepository.UpdateUserProfile(userProfileDetails,AvatarUrl);
        }
    }
}
