using API.Contracts.Repositories;
using API.Contracts.Services;
using API.Entities.Dedicated.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class PostRepository : IPostRepository
    {
        public readonly IDataService _dataService;
        public PostRepository(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<List<GetAllPosts>> GetPosts()
        {
            var query = @"
                    SELECT 
                        p.Id AS PostId,
                        p.Title AS PostTitle,
                        p.Tags AS PostTags,
                        p.Slug AS PostSlug,
                        p.[Description] AS PostDescription,
                        p.DateAdded AS DateAdded,
                        c.Title AS CategoryTitle,
                        c.Description AS CategoryDescription
                    FROM 
                        tblPosts p
                    INNER JOIN 
                        tblPostCategories c
                    ON 
                        p.CategoryId = c.Id;
                ";

            var posts = await _dataService.QueryAsync<GetAllPosts>(query);
            return posts.ToList();
        }

    }
}
