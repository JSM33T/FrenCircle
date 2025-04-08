namespace FrenCircle.Contracts.Dtos.Requests
{
    public class ChangeLogRequestDto
    {
        public int Id { get; set; }
        public string Version { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ChangeType { get; set; }
        public string Contributors { get; set; }
        public DateTime ChangedAt { get; set; }
        public Guid RowId { get; set; }
    }
    public class ChangeLogBulkRequestDto
    {
        public string Version { get; set; }
        public List<ChangeLogItemDto> Changes { get; set; }
        public int? ChangedBy { get; set; }
    }

    public class ChangeLogItemDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ChangeType { get; set; }
        public string Contributors { get; set; }
    }
}
