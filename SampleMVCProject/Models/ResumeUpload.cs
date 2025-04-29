using System.ComponentModel.DataAnnotations;

namespace SampleMVCProject.Models
{
    public class ResumeUpload
    {
        public long Id { get; set; }
        public string? Resume { get; set; }
        public IFormFile UploadResume { get; set; }
    }
}

