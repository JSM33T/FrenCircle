namespace FrenCircle.Contracts.Dtos
{
    public record ApiResponse<T>(int Status, string Message, T Data, List<string>? Hints = null)
    {
        public List<string> Hints { get; init; } = Hints ?? [];
        public long? ResponseTimeMs { get; set; } // ✅ This should be serialized
    }

}
