using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SampleMVCProject.Models
{
    public class Users:IdentityUser
    {
        public char Gender { get; set; }
        public string Department { get; set; }
        public string ProfileImagePath {  get; set; }
        public string  Password { get; set; }
    }
}
