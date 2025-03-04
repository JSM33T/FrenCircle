using Dapper;
using FrenCircle.Entities.Data;
using FrenCircle.Helpers.SqlQueries;
using FrenCircle.Infra;

namespace FrenCircle.Repositories
{
    public interface IPostRepository
    {
        Task<int> AddPost(AddPostRequest postRequest);
        Task<bool> EditPost(Post post);
        Task<bool> DeletePost(int postId);
        Task<bool> PublishPost(int postId);
    }

    public class PostRepository(IDapperFactory dapperFactory) : IPostRepository
    {
        public async Task<int> AddPost(AddPostRequest postRequest)
        {
            var query = DbPosts.AddPost;
            
            var id = await dapperFactory.GetData<int>(query, new
            {
                postRequest.Title,
                postRequest.Description,
                postRequest.Slug,
                postRequest.ImageSlug,
                postRequest.Content,
                postRequest.Tags,
                postRequest.AuthorId,
                postRequest.DateAdded
            });
            
            return id;
        }

        public async Task<bool> EditPost(Post post)
        {
            var query = DbPosts.EditPost;
            
            var rowsAffected = await dapperFactory.Execute(query, new
            {
                post.Title,
                post.Description,
                post.Slug,
                post.ImageSlug,
                post.Content,
                post.Tags,
                post.DateEdited,
                post.Id
            });
            
            return rowsAffected > 0;
        }

        public async Task<bool> DeletePost(int postId)
        {
            var query = DbPosts.DeletePost;
            
            var rowsAffected = await dapperFactory.Execute(query, new { PostId = postId });
            
            return rowsAffected > 0;
        }

        public async Task<bool> PublishPost(int postId)
        {
            var query = DbPosts.PublishPost;
            
            var rowsAffected = await dapperFactory.Execute(query, new { PostId = postId });
            
            return rowsAffected > 0;
        }
    }
}
