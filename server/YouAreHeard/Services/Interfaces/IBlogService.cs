using YouAreHeard.Models;

namespace YouAreHeard.Services.Interfaces
{
    public interface IBlogService
    {
        void uploadBlog(BlogDTO blog);

        List<BlogDTO> getAllBlogs();
    }
}