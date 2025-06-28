using Microsoft.AspNetCore.Mvc;
using YouAreHeard.Models;
using YouAreHeard.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class BlogController : ControllerBase
{
    private IBlogService _blogService;

    public BlogController(IBlogService blogService)
    {
        _blogService = blogService;
    }

    [HttpGet("all")]
    public IActionResult GetBlogs()
    {
        var blogs = _blogService.getAllBlogs();

        return Ok(blogs);
    }

    [HttpPost("postblog")]
    public IActionResult postBlog([FromBody] BlogDTO blog)
    {
        if (!ModelState.IsValid)
        {
            {
                return BadRequest(ModelState);
            }
        }
        _blogService.UploadBlog(blog);
        return Ok();
    }

    [HttpPut("updateblog")]
    public IActionResult UpdateBlog([FromBody] BlogDTO blog)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        _blogService.UpdateBlog(blog);
        return Ok();
    }

    [HttpDelete("deleteblog/{blogId}")]
    public IActionResult DeleteBlog(int blogId)
    {
        if (blogId <= 0)
        {
            return BadRequest("Invalid blog ID.");
        }
        _blogService.DeleteBlog(blogId);
        return Ok();
    }

    [HttpGet("userblogs/{userId}")]
    public IActionResult GetBlogsByUserId(int userId)
    {
        if (userId <= 0)
        {
            return BadRequest("Invalid user ID.");
        }
        var blogs = _blogService.GetBlogsByUserId(userId);
        if (blogs == null || blogs.Count == 0)
        {
            return NotFound("No blogs found for this user.");
        }
        return Ok(blogs);
    }
}