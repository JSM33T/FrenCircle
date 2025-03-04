namespace FrenCircle.Helpers.SqlQueries
{
    /// <summary>
    /// Contains SQL queries for post operations.
    /// </summary>
    public static class DbPosts
    {
        /// <summary>
        /// Query to get all posts.
        /// </summary>
        public const string GetAllPosts = @"
            SELECT *
            FROM Posts";

        /// <summary>
        /// Query to add a new post.
        /// </summary>
        public const string AddPost = @"
            INSERT INTO Posts (
                Title, 
                [Description], 
                Slug, 
                ImageSlug, 
                Content, 
                Tags, 
                AuthorId,
                DateAdded
            ) VALUES (
                @Title, 
                @Description, 
                @Slug, 
                @ImageSlug, 
                @Content, 
                @Tags, 
                @AuthorId,
                @DateAdded
            );
            SELECT SCOPE_IDENTITY();";

        /// <summary>
        /// Query to edit an existing post.
        /// </summary>
        public const string EditPost = @"
            UPDATE Posts
            SET Title = @Title,
                [Description] = @Description,
                Slug = @Slug,
                ImageSlug = @ImageSlug,
                Content = @Content,
                Tags = @Tags,
                DateEdited = @DateEdited
            WHERE Id = @Id;";

        /// <summary>
        /// Query to delete a post by ID.
        /// </summary>
        public const string DeletePost = @"
            DELETE FROM Posts
            WHERE Id = @PostId;";

        /// <summary>
        /// Query to publish a post (set IsPublished = true).
        /// </summary>
        public const string PublishPost = @"
            UPDATE Posts
            SET IsPublished = 1
            WHERE Id = @PostId;";
    }
}
