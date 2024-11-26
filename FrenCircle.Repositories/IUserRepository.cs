using FrenCircle.Entities.Fren;

namespace FrenCircle.Repositories
{
    public interface IUserRepository
    {
        public Task<Fren?> GetUserByCredentials(FrenLoginRequest loginRequest);
        public Task<FrenLoginResponse?> LoginUser(FrenLoginRequest loginRequest);
        public Task SignUpFren(FrenSignUpRequest signUpRequest);
        public Task<int> VerifyFren(FrenVerificationRequest verificationRequest);
    }
}
