using System.ComponentModel.DataAnnotations;

namespace Blogging_Platform_Backend.DTOModels.User
{
    public class dtoLogin
    {
        [Required]
        public string usernameEmail { get; set; }
        [Required]
        public string password { get; set; }
    }
}
