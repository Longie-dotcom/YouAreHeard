using Microsoft.AspNetCore.Mvc;

namespace YouAreHeard.Models
{
    public class FileUploadDTO
    {
        [FromForm(Name = "file")]
        public IFormFile File { get; set; }
    }
}
