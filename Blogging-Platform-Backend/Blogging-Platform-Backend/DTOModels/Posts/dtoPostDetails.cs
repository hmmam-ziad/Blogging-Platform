using Blogging_Platform_Backend.Models;

namespace Blogging_Platform_Backend.DTOModels.Posts
{
    public class dtoPostDetails
    {
        public string PostId { get; set; }
        public string AutherName { get; set; }
        public string PostTitle { get; set; }
        public string PostContent { get; set; }
        public DateTime? DateCreated { get; set; }
        public int LikeCount { get; set; }
        public string? CoverImageUrl { get; set; }
        public int CommentCount { get; set; }

        public ICollection<dtoPostComments> Comments { get; set; }
    }
}
