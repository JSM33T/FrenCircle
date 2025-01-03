namespace API.Entities.Dedicated.Contact
{
    public class ContactRequest
    {
        public int Id { get; set; }
        public int FrenId { get; set; } = 0;
        public string Name { get; set; }
        public string Origin { get; set; } = "na";
        public string Message { get; set; }
    }
}
