using System.ComponentModel.DataAnnotations;

namespace Blogging_Platform_Backend.Models
{
    public class Tag
    {
        [Key]
        public string TagId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; }

        public ICollection<PostTag>? PostTags { get; set; }
    }
}
