using System.ComponentModel.DataAnnotations;

namespace SampleMVCProject.Models
{
    public class EmployeeLoginDeatails
    {
        [Required(ErrorMessage="Please Enter Username")]
        [Display(Name ="UserName")]
        public string Username { set; get; }
        [Required(ErrorMessage ="Please Enter Password")]
        [Display(Name ="Password")]
        public string Password {  set; get; }
    }
}
