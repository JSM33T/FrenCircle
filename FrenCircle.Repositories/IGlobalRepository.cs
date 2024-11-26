namespace FrenCircle.Repositories
{
    public interface IGlobalRepository
    {
        public Task<string> GetGlobalValue(string key);
    }
}
