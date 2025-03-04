using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenCircle.Entities.Data
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ActorId { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public string NotificationType { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? ActionUrl { get; set; }
        public bool IsRead { get; set; }
        public bool EmailSent { get; set; }
        public DateTime? EmailSentAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateNotificationDto
    {
        public int UserId { get; set; }
        public int ActorId { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public string NotificationType { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? ActionUrl { get; set; }
    }


}
