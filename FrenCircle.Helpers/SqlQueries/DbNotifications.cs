namespace FrenCircle.Helpers.SqlQueries
{
    public static class DbNotifications
    {
        public static string AddNotification => @"
        INSERT INTO Notifications (UserId, ActorId, PostId, CommentId, NotificationType, Message, ActionUrl)  
VALUES (@UserId, @ActorId, @PostId, @CommentId, @NotificationType, @Message, @ActionUrl);
";

        public static string GetNotifsForAUser => @"
        SELECT * FROM Notifications  
        WHERE UserId = @UserId  
        ORDER BY CreatedAt DESC;        
        ";
    }
}
