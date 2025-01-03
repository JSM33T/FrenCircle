using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Entities.Dedicated.Content
{
    public class ContentResponse
    {
        public string Id{ get; set; }

        public Guid ContentKey { get; set; }

        public string ContentData { get; set; }

        public string Slug { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        public string Cover { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
