using System.ComponentModel.DataAnnotations;

namespace Blogging_Platform_Backend.Models
{
    public class PostTag
    {
        [Key]
        public string PostTagId { get; set; } = Guid.NewGuid().ToString();
        public string PostId { get; set; }
        public Post? Post { get; set; }

        public string TagId { get; set; }
        public Tag? Tag { get; set; }
    }
}