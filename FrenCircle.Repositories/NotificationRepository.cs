using FrenCircle.Entities.Data;
using FrenCircle.Helpers.SqlQueries;
using FrenCircle.Infra;

namespace FrenCircle.Repositories
{
    public interface INotificationRepository
    {
        public Task AddNotification(CreateNotificationDto notificationDto);
    }
    public class NotificationRepository(IDapperFactory dapperFactory) : INotificationRepository
    {
        public async Task AddNotification(CreateNotificationDto notificationDto)
        {
            var query = DbNotifications.AddNotification;

            var notification = new Notification
            {
                UserId = notificationDto.UserId,
                ActorId = notificationDto.ActorId,
                PostId = notificationDto.PostId,
                CommentId = notificationDto.CommentId,
                NotificationType = notificationDto.NotificationType,
                Message = notificationDto.Message,
                ActionUrl = notificationDto.ActionUrl,
                CreatedAt = DateTime.UtcNow
            };

            var id = await dapperFactory.GetData<int>(query, new
            {
                notification.UserId,
                notification.ActorId,
                notification.PostId,
                notification.CommentId,
                notification.NotificationType,
                notification.Message,
                notification.ActionUrl
            });

            notification.Id = id;
        }
    }
}
