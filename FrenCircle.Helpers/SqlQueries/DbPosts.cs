namespace FrenCircle.Helpers.SqlQueries
{
    /// <summary>
    /// Contains SQL queries for login operations.
    /// </summary>
    public static class DbPosts
    {
        /// <summary>
        /// Query to get login information by DeviceId.
        /// </summary>
        public const string GetAllPosts = @"
            SELECT *
            FROM Posts";

        public const string AddPost = @"
            INSERT INTO Posts (
                Title, 
                [Description], 
                Slug, 
                ImageSlug, 
                Content, 
                Tags, 
                IsPublished, 
                AuthorId
            ) VALUES (
                @Title, 
                @Description, 
                @Slug, 
                @ImageSlug, 
                @Content, 
                @Tags, 
                @IsPublished, 
                @AuthorId
            );
        ";
    }
}
