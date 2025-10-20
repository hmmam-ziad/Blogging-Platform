using Blogging_Platform_Backend.Data;
using Blogging_Platform_Backend.DTOModels.Comments;
using Blogging_Platform_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blogging_Platform_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentsController(AppDbContext context)
        {
            _context = context;
        }

        // we gonna implement GetCommentsByPost/{postId} if we need later.
        [Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddComment(dtoComment model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var postExists = await _context.Posts.AnyAsync(p => p.PostId == model.PostId);
                if (!postExists) 
                    return NotFound();  
                var comment = new Comment
                {
                    Content = model.Content,
                    UserId = userId!,
                    PostId = model.PostId,
                    CreatedAt = DateTime.UtcNow,
                };

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Your comment was added successfully" });
            }
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteComment(string Id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.CommentId == Id);
            if (comment == null)
                return NotFound();

            if (comment.UserId != currentUserId)
                return Forbid();

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return Ok(new {message = "Your comment was Deleted successfully" });
        }
    }
}
