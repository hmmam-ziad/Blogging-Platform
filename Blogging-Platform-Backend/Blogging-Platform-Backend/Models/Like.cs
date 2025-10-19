using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blogging_Platform_Backend.Models
{
    public class Like
    {
        [Key]
        public string LikeId { get; set; } = Guid.NewGuid().ToString();

        [ForeignKey(nameof(Post))]
        public string PostId { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public Post? Post { get; set; }
        public UserApp? User { get; set; }
    }
}