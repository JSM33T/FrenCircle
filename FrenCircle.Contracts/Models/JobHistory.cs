namespace FrenCircle.Contracts.Models
{
    public class JobHistory
    {
        public int Id { get; set; }
        public string JobName { get; set; } = default!;
        public string Status { get; set; } = default!;
        public DateTime ScheduledAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int? DurationMs { get; set; }
        public string? Error { get; set; }
        public string? TriggeredBy { get; set; }
        public string? Metadata { get; set; }
    }

}
