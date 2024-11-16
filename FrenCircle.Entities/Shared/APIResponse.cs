namespace FrenCircle.Entities.Shared
{
    public class APIResponse<T>(int status, string message, T data, List<string> hints)
    {
        public int Status { get; set; } = status;
        public string Message { get; set; } = message;
        public List<string> Hints { get; set; } = hints ?? [];
        public T Data { get; set; } = data;
    }
}