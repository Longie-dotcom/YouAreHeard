using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;
using YouAreHeard.Services.Interfaces;

namespace YouAreHeard.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;

        public BlogService(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }
        public void uploadBlog(BlogDTO blog)
        {
            _blogRepository.UploadBlog(blog);
        }

        public List<BlogDTO> getAllBlogs()
        {
            return _blogRepository.getAllBlogs();
        }
    }
}
