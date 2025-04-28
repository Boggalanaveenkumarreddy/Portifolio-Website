using System.ComponentModel.DataAnnotations;

namespace SampleMVCProject.Models
{
    public class ProfileUpload
    {
        public long Id { get; set; }
        public string ?Profile { get; set; }
        [Required(ErrorMessage = "Please upload a profile picture")]
        public IFormFile UploadProfile { get; set; }  
    }
}
