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

    [HttpPost("upload-image")]
    public async Task<IActionResult> UploadImage([FromForm] FileUploadDTO model)
    {
        var file = model.File;

        if (file == null || file.Length == 0)
            return BadRequest(new { message = "No file uploaded." });

        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "blog-images");
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var fullPath = Path.Combine(folderPath, fileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var relativePath = $"/uploads/blog-images/{fileName}";
        return Ok(new { fileName, url = relativePath });
    }
}