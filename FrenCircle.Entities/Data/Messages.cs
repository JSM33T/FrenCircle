namespace FrenCircle.Entities.Data
{
    public class Message
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Text { get; set; }
        public DateTime DateAdded { get; set; }
    }

    public class AddMessageRequest
    {
        public string? Name { get; set; }
        public required string Email { get; set; }
        public required string Text { get; set; }
    }
}
