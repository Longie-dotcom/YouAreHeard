using YouAreHeard.Models;

namespace YouAreHeard.Services.Interfaces
{
    public interface IBlogService
    {
        void UploadBlog(BlogDTO blog);

        List<BlogDTO> getAllBlogs();

        void UpdateBlog(BlogDTO blog);
        void DeleteBlog(int blogId);
        List<BlogDTO> GetBlogsByUserId(int userId);
    }
}