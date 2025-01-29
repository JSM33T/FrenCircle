using FrenCircle.Entities.Data;

namespace FrenCircle.Helpers.Mappers
{
    public static partial class UserDtoMappers
    {
        public static User MAP_AddUserRequest_User(AddUserRequest addUserRequest)
        {
            var user = new User
            {
                FirstName = addUserRequest.FirstName,
                LastName = addUserRequest.LastName ?? string.Empty,
                UserName = addUserRequest.UserName,
                Email = addUserRequest.Email,
                Bio = addUserRequest.Bio ?? string.Empty,
                PasswordHash = null,
            };
            
            return user;
        }

    }
}
