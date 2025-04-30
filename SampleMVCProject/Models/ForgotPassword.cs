using System.ComponentModel.DataAnnotations;

namespace SampleMVCProject.Models
{
    public class ForgotPassword
    {
        [Required(ErrorMessage ="Please enter Username or emailId")]
        [EmailAddress(ErrorMessage = "Invalid Username or Email")]
        public string Username { get; set; }
    }
}
