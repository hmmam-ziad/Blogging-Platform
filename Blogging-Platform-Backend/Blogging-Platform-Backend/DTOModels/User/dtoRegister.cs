using System.ComponentModel.DataAnnotations;

namespace Blogging_Platform_Backend.DTOModels.User
{
    public class dtoRegister
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string? FullName { get; set; }
    }
}
