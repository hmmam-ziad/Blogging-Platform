using Blogging_Platform_Backend.Data;
using Blogging_Platform_Backend.DTOModels.Posts;
using Blogging_Platform_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blogging_Platform_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<UserApp> _userManager;

        public PostsController(AppDbContext context, UserManager<UserApp> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllPosts() {
            var posts = from p in _context.Posts
                        select new dtoPost()
                        {
                            PostId = p.PostId,
                            Content = p.Content,
                            CommentCount = p.Comments!.Count(),
                            LikeCount = p.Likes!.Count(),
                            DateCreated = p.DateCreated,
                            AutherName = p.Author!.FullName ?? p.Author.UserName!,
                            CoverImageUrl = p.CoverImageUrl,
                        };
            return Ok(posts);
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetPostById(string id)
        {
            var post = await _context.Posts.Include(u => u.Author).Include(c => c.Comments).Include(l => l.Likes).Select(p =>
                new dtoPostDetails()
                {
                    PostId = p.PostId,
                    PostContent = p.Content,
                    PostTitle = p.Title,
                    AutherName = p.Author!.FullName ?? p.Author.UserName!,
                    CoverImageUrl = p.CoverImageUrl,
                    DateCreated = p.DateCreated,
                    CommentCount= p.Comments!.Count(),
                    LikeCount= p.Likes!.Count(),
                    Comments = p.Comments.Select(c => new dtoPostComments()
                    {
                        CommentId = c.CommentId,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt,
                        AuthorName = c.User!.FullName ?? c.User.UserName!
                    }).ToList(),
                }
            ).SingleOrDefaultAsync(p => p.PostId == id);

            if(post is null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [Authorize]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetMyPosts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var posts = await _context.Posts
                .Where(p => p.AutherId == userId)
                .Select(p => new dtoPost
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Content = p.Content,
                    CommentCount = p.Comments!.Count(),
                    LikeCount = p.Likes!.Count(),
                    DateCreated = p.DateCreated,
                    AutherName = p.Author!.FullName ?? p.Author.UserName!,
                    CoverImageUrl = p.CoverImageUrl,
                })
                .ToListAsync();
            return Ok(posts);
        }

        [Authorize]
        [HttpPost("[action]")]
        public async Task<IActionResult> CreatePost(dtoCreatePost post)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var Post = new Post
                {
                    Title = post.Title,
                    Content = post.Content,
                    CoverImageUrl = post.CoverImageUrl,
                    AutherId = userId,
                    DateCreated = DateTime.Now,
                };

                _context.Posts.Add(Post);
                await _context.SaveChangesAsync();
                return Ok(post);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> UpdatePost(string id, dtoUpdatePost model)
        {
            try
            {
                var post = await _context.Posts.FindAsync(id);
                if (post is null)
                    return NotFound("Post not Found");
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (post.AutherId != userId)
                    return Forbid("You Cannot Edit someone else`s post.");
                post.Title = model.Title;
                post.Content = model.Content;
                post.CoverImageUrl = model.CoverImageUrl;
                post.DateUpdated = DateTime.Now;

                await _context.SaveChangesAsync();
                return Ok(post);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeletePost(string id)
        {
            try
            {
                var post = await _context.Posts.FindAsync(id);

                if (post is null)
                    return NotFound("Post not found");

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (post.AutherId != userId)
                    return Forbid("You cannot delete someone else's post.");

                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();

                return Ok(post);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
