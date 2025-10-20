using Blogging_Platform_Backend.Models;

namespace Blogging_Platform_Backend.DTOModels.Posts
{
    public class dtoPostComments
    {
        public string CommentId { get; set; }
        public string Content { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserApp? User { get; set; }
    }
}
