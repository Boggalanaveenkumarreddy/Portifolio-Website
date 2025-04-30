
using System.ComponentModel.DataAnnotations;

namespace SampleMVCProject.Models
{
    public class ContactUs
    {
        [Display(Name = "Full Name")]
        [StringLength(100, MinimumLength =3, ErrorMessage = "Full Name cannot be  lessthan 3 characters")]
        public required string FullName { get; set; }

        [Required(ErrorMessage = "Please enter Phone Number")]
        [RegularExpression(@"^[1-9]\d{9}$", ErrorMessage = "Phone number must be 10 digits and cannot start with 0.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Message")]
        [StringLength(250, ErrorMessage = "Message cannot exceed 250 characters")]
        public string? Message { get; set; }

        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public required string Email { get; set; }
    }
}
