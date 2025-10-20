using Blogging_Platform_Backend.Data;
using Blogging_Platform_Backend.DTOModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blogging_Platform_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }


        // This is way to Change User Data is not secure, you must check if he has a real email and phone number by confirme them.
        [Authorize]
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateProfile(string Id, dtoUser model)
        {
            try
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (currentUserId != Id)
                {
                    return Forbid();
                }
                var user = await _context.Users.FindAsync(Id);

                if (user is null)
                {
                    return NotFound();
                }

                user.UserName = model.UserName;
                user.FullName = model.FullName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;

                await _context.SaveChangesAsync();
                return Ok(new { user.Id, user.UserName, user.FullName, user.Email, user.PhoneNumber });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
    }
}
