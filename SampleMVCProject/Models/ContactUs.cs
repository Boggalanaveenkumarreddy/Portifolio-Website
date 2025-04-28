
using System.ComponentModel.DataAnnotations;

namespace SampleMVCProject.Models
{
    public class ContactUs
    {
        [Display(Name = "Full Name")]
        [StringLength(100, ErrorMessage = "Full Name cannot be longer than 100 characters")]
        public required string FullName { get; set; }

        [Display(Name = "Phone Number")]
        [Range(1000000000, 999999999999, ErrorMessage = "Please enter a valid phone number")]
        public long PhoneNumber { get; set; }

        [Display(Name = "Message")]
        [StringLength(250, ErrorMessage = "Message cannot exceed 250 characters")]
        public string? Message { get; set; }

        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public required string Email { get; set; }
    }
}
