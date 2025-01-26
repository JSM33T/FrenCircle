namespace FrenCircle.Entities.Data
{
    public class Message
    {
        public int Id { get; set; }
        public required string Name { get; init; }
        public required string Email { get; init; }
        public required string Text { get; init; }
    }

    public class AddMessageRequest
    {
        public string? Name { get; set; }
        public required string Email { get; set; }
        public required string Text { get; set; }
    }
}
