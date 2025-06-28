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
        public void UploadBlog(BlogDTO blog)
        {
            _blogRepository.UploadBlog(blog);
        }

        public List<BlogDTO> getAllBlogs()
        {
            return _blogRepository.getAllBlogs();
        }

        public void UpdateBlog(BlogDTO blog)
        {
            _blogRepository.UpdateBlog(blog);
        }

        public void DeleteBlog(int blogId)
        {
            _blogRepository.DeleteBlog(blogId);
        }

        public List<BlogDTO> GetBlogsByUserId(int userId)
        {
            return _blogRepository.GetBlogsByUserId(userId);
        }
    }
}