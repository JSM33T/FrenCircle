namespace API.Infra
{
    public interface ICommonService
    {
        public Task<bool> SendNotification(string Identifier,string Message);
    }
}
