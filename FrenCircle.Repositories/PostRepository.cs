using FrenCircle.Entities.Data;
using FrenCircle.Helpers.Mappers;
using FrenCircle.Helpers.SqlQueries;
using FrenCircle.Infra;

namespace FrenCircle.Repositories
{
    public interface IPostRepository {
        Task<Post> AddPost(AddPostRequest addPostRequest);
        public Task<IEnumerable<Post>> GetAllPosts();
    }

    public class PostRepository(IDapperFactory dapperFactory) : IPostRepository
    {
        private readonly IDapperFactory _dapperFactory = dapperFactory;

        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            var query = DbPosts.GetAllPosts;
            var user = await _dapperFactory.GetDataList<Post>(query);
            return user;
        }

        public async Task<Post> AddPost(AddPostRequest postRequest)
        {
            var post = PostMappers.MAP_AddPostRequest_Post(postRequest);


            var query = DbPosts.AddPost;

            await dapperFactory.GetData<int>(query, new
            {
                post.Title,
                post.Description,
                post.Slug,
                post.ImageSlug,
                post.Content,
                post.Tags,
                post.IsPublished,
                post.AuthorId
            });

            return post;

        }
    }
}
