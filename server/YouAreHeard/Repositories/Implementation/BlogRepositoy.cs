using YouAreHeard.Models;
using YouAreHeard.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using YouAreHeard.Enums;
using YouAreHeard.NewFolder;

namespace YouAreHeard.Repositories.Implementation
{
    public class BlogRepository : IBlogRepository
    {
        public List<BlogDTO> getAllBlogs()
        {
            using var conn = DBContext.GetConnection();
            conn.Open();

            string query = @"
            SELECT
                b.blogID,
                b.userID,
                b.caption,
                b.title,
                b.image,
                b.date
            FROM Blog b
            ";
            using var cmd = new SqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            var blogs = new List<BlogDTO>();

            while (reader.Read())
            {
                blogs.Add(new BlogDTO
                {
                    blogId = reader.GetInt32(reader.GetOrdinal("blogID")),
                    userId = reader.GetInt32(reader.GetOrdinal("userID")),
                    caption = reader.GetString(reader.GetOrdinal("caption")),
                    title = reader.GetString(reader.GetOrdinal("title")),
                    image = reader.GetString(reader.GetOrdinal("image")),
                    date = reader.GetDateTime(reader.GetOrdinal("date"))
                });
            }
            return blogs;
            
        }

        public void UploadBlog(BlogDTO blog)
        {
            using var conn = DBContext.GetConnection();

            conn.Open();

            string query = @"
                INSERT INTO Blog (blogID, userID, caption, title, image, date)
                Values (@blogID, @userID, @caption, @title, @image, @date)
            ";

            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@blogID", blog.blogId);
            cmd.Parameters.AddWithValue("@userID", blog.userId);
            cmd.Parameters.AddWithValue("@caption", blog.caption);
            cmd.Parameters.AddWithValue("@title", blog.title);
            cmd.Parameters.AddWithValue("@image", blog.image);
            cmd.Parameters.AddWithValue("@date", blog.date);
            cmd.ExecuteNonQuery();
        }
    }
}