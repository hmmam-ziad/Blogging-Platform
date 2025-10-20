using Blogging_Platform_Backend.Data;
using Blogging_Platform_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blogging_Platform_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LikesController(AppDbContext context)
        {
             _context = context;
        }

        [Authorize]
        [HttpPost("{postId}/toggle")]
        public async Task<IActionResult> ToggleLike(string postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

            if (existingLike is not null)
            {
                _context.Likes.Remove(existingLike);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Like removed" });
            }

            var like = new Like
            {
                PostId = postId,
                UserId = userId,
            };

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Post liked" });
        }
    }
}
