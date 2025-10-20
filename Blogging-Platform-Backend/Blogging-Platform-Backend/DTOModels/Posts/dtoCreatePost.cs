using System.ComponentModel.DataAnnotations;

namespace Blogging_Platform_Backend.DTOModels.Posts
{
    public class dtoCreatePost
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; } // Rich Text HTML

        public string? CoverImageUrl { get; set; }
    }

}
