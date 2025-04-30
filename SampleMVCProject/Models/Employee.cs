using System.ComponentModel.DataAnnotations;

namespace SampleMVCProject.Models
{
    public class Employee
    {
        public long Id { get; set; }
        [Required(ErrorMessage ="Please enter User Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter Phone Number")]
        [RegularExpression(@"^[1-9]\d{9}$", ErrorMessage = "Phone number must be 10 digits and cannot start with 0.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Please enter EmailId ")]
        [EmailAddress(ErrorMessage = "Enter valid emailId")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter Password")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must atleast 6 characters")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please select Gender")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Please select Department")]
        public string Department { get; set; }
        public string? Profile { get; set; }
        public string? Resume { get; set; }

        public IFormFile? UploadResume { get; set; }
    }
}
