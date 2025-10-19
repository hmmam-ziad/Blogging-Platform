using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blogging_Platform_Backend.Models
{
    public class Post
    {
        [Key]
        public string PostId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; } // Rich Text HTML

        public string? CoverImageUrl { get; set; }

        public DateTime? DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateUpdated { get; set; }
        [ForeignKey(nameof(Author))]
        public string AutherId { get; set; }
        // Navigation
        public UserApp? Author { get; set; }

        public ICollection<Comment>? Comments { get; set; }
        public ICollection<PostTag>? PostTags { get; set; }
        public ICollection<Like>? Likes { get; set; }
    }
}