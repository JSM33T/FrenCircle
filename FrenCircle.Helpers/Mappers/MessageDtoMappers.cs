using FrenCircle.Entities.Data;

namespace FrenCircle.Helpers.Mappers
{
    public static class MessageDtoMappers
    {
        public static Message MAP_AddMessageRequest_Message(AddMessageRequest messageRequest)
        {
            return new Message
            {
                Name = messageRequest.Name ?? string.Empty,
                Email = messageRequest.Email,
                Text = messageRequest.Text,
                DateAdded = DateTime.UtcNow
            };
        }

    }
}
