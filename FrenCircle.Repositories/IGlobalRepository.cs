namespace FrenCircle.Repositories
{
    public interface IGlobalRepository
    {
        public Task<string> GetGLobalValue(string Key);
    }
}
