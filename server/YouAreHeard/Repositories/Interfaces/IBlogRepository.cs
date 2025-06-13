using YouAreHeard.Models;

namespace YouAreHeard.Repositories.Interfaces
{
    public interface IBlogRepository
    {
        public void UploadBlog(BlogDTO blog);

        public List<BlogDTO> getAllBlogs();
    }
}