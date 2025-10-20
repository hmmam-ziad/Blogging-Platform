using System.ComponentModel.DataAnnotations;

namespace Blogging_Platform_Backend.DTOModels.Posts
{
    public class dtoPost
    {
        public string PostId { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; } // Rich Text HTML

        public string? CoverImageUrl { get; set; }

        public DateTime? DateCreated { get; set; }
        public string AutherName { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
    }
}
