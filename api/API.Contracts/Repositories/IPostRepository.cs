using API.Entities.Dedicated.Posts;

namespace API.Contracts.Repositories
{
    public interface IPostRepository
    {
        public Task<List<GetAllPosts>> GetPosts();

    }
}
