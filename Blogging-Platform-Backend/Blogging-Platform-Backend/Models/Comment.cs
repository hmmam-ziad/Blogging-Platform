using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blogging_Platform_Backend.Models
{
    public class Comment
    {
        [Key]
        public string CommentId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        [ForeignKey(nameof(Post))]
        public string PostId { get; set; }

        public UserApp? User { get; set; }
        public Post? Post { get; set; }
        
    }
}