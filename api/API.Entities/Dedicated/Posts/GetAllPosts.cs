using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Entities.Dedicated.Posts
{
    public class GetAllPosts
    {
        public int PostId { get; set; }
        public string PostTags { get; set; }
        public string PostDescription { get; set; }
        public string PostTitle { get; set; }
        public DateTime DateAdded { get; set; }
        public string PostSlug { get; set; }
        public string CategoryTitle { get; set; }
    }
}
