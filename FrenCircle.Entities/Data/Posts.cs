using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrenCircle.Entities.Data
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = "Description here";

        public string Slug { get; set; } = string.Empty;

        public Guid RowId { get; set; } = Guid.NewGuid();

        public string? ImageSlug { get; set; }

        public string Content { get; set; } = "#draft";

        public string Tags { get; set; } = "#fc";

        public DateTime DateAdded { get; set; } = DateTime.Now;

        public DateTime DateEdited { get; set; } = DateTime.Now;

        public bool IsPublished { get; set; } = false;

        public int AuthorId { get; set; }
    }

    public class AddPostRequest
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; }

        public string Slug { get; set; }

        public string ImageSlug { get; set; }

        public string Content { get; set; }

        public string Tags { get; set; }
        
        public int AuthorId { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;

    }

}
