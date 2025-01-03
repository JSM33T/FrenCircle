namespace API.Entities.Shared
{
    public class APIResponse<T>(int status, string message, T data, List<string> hints = null)
    {
        public int Status { get; set; } = status;
        public string Message { get; set; } = message;
        public T Data { get; set; } = data;
        public List<string> Hints { get; set; } = hints ?? [];
        
    }
}
