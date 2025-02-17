using FrenCircle.Entities.Data;

namespace FrenCircle.Helpers.Mappers
{
    public static partial class PostMappers
    {
        public static Post MAP_AddPostRequest_Post(AddPostRequest addPostRequest)
        {
            return new Post
            {
                Slug =  addPostRequest.Slug,
                Title = addPostRequest.Title,
                Content = addPostRequest.Content,
                ImageSlug = addPostRequest.ImageSlug,
                Description = addPostRequest.Description,
                IsPublished = false,
            };
        }
    }
}
