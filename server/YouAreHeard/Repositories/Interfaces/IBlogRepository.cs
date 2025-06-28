using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IBlogRepository
    {
        public void UploadBlog(BlogDTO blog);

        public List<BlogDTO> getAllBlogs();

        public void UpdateBlog(BlogDTO blog);
        public void DeleteBlog(int blogId);
        public List<BlogDTO> GetBlogsByUserId(int userId);
    }
}