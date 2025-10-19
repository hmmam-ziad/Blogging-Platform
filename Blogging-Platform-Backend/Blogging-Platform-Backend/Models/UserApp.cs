using Microsoft.AspNetCore.Identity;

namespace Blogging_Platform_Backend.Models
{
    public class UserApp : IdentityUser
    {
        public string? FullName { get; set; }
        // Navigation 
        public ICollection<Post>? Posts { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Like>? Likes { get; set; }
    }
}
