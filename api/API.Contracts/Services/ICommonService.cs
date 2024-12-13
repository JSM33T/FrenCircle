namespace API.Contracts.Services
{
    public interface ICommonService
    {
        public Task<bool> SendNotification(string Identifier, string Message);
    }
}
