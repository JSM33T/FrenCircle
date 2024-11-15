using FrenCircle.Entities.Fren;

namespace FrenCircle.Repositories
{
    public interface IUserRepository
    {
        public Task<Fren?> GetUserByCredentials(FrenLoginRequest loginRequest);
    }
}
