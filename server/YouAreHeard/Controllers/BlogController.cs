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
        _blogService.uploadBlog(blog);
        return Ok();
    }
}