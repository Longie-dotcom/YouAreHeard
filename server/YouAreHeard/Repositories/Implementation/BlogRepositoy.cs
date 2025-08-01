using Microsoft.Data.SqlClient;
using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;

namespace YouAreHeard.Repositories.Implementation
{
    public class BlogRepository : IBlogRepository
    {
        public List<BlogDTO> getAllBlogs()
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
            SELECT b.blogID, b.userID, b.details, b.title, b.image, b.date,
                   u.name
            FROM Blog b
            INNER JOIN [User] u ON b.userID = u.userID
            ORDER BY b.date DESC";

            using var cmd = new SqlCommand(query, conn);

            using var reader = cmd.ExecuteReader();
            var blogs = new List<BlogDTO>();

            while (reader.Read())
            {
                var blog = new BlogDTO
                {
                    blogId = reader.GetInt32(reader.GetOrdinal("blogID")),
                    userId = reader.GetInt32(reader.GetOrdinal("userID")),
                    details = reader.GetString(reader.GetOrdinal("details")),
                    title = reader.GetString(reader.GetOrdinal("title")),
                    image = reader.GetString(reader.GetOrdinal("image")),
                    date = reader.GetDateTime(reader.GetOrdinal("date")),
                    UserName = reader.GetString(reader.GetOrdinal("name"))
                };
                blogs.Add(blog);
            }

            return blogs;
        }

        public void UploadBlog(BlogDTO blog)
        {
            using var conn = DBContext.GetConnection();

            conn.Open();

            string query = @"
                INSERT INTO Blog (userID, details, title, image, date)
                Values (@userID, @details, @title, @image, @date)
            ";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userID", blog.userId);
            cmd.Parameters.AddWithValue("@details", blog.details);
            cmd.Parameters.AddWithValue("@title", blog.title);
            cmd.Parameters.AddWithValue("@image", blog.image);
            cmd.Parameters.AddWithValue("@date", blog.date);
            cmd.ExecuteNonQuery();
        }

        public void UpdateBlog(BlogDTO blog)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
                UPDATE Blog
                SET 
                    details = @details,
                    title = @title,
                    image = @image,
                    date = @date
                WHERE blogID = @blogID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@blogID", blog.blogId);
            cmd.Parameters.AddWithValue("@details", blog.details);
            cmd.Parameters.AddWithValue("@title", blog.title);
            cmd.Parameters.AddWithValue("@image", blog.image);
            cmd.Parameters.AddWithValue("@date", blog.date);

            cmd.ExecuteNonQuery();
        }

        public void DeleteBlog(int blogId)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = "DELETE FROM Blog WHERE blogID = @blogID";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@blogID", blogId);

            cmd.ExecuteNonQuery();
        }

        public List<BlogDTO> GetBlogsByUserId(int userId)
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
            SELECT b.blogID, b.userID, b.details, b.title, b.image, b.date,
                   u.name
            FROM Blog b
            INNER JOIN [User] u ON b.userID = u.userID
            WHERE b.userID = @userID
            ORDER BY b.date DESC";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userID", userId);

            using var reader = cmd.ExecuteReader();
            var blogs = new List<BlogDTO>();

            while (reader.Read())
            {
                var blog = new BlogDTO
                {
                    blogId = reader.GetInt32(reader.GetOrdinal("blogID")),
                    userId = reader.GetInt32(reader.GetOrdinal("userID")),
                    details = reader.GetString(reader.GetOrdinal("details")),
                    title = reader.GetString(reader.GetOrdinal("title")),
                    image = reader.GetString(reader.GetOrdinal("image")),
                    date = reader.GetDateTime(reader.GetOrdinal("date")),
                    UserName = reader.GetString(reader.GetOrdinal("name"))
                };
                blogs.Add(blog);
            }

            return blogs;
        }
    }
}