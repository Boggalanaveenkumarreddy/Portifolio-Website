using System.ComponentModel.DataAnnotations;

namespace SampleMVCProject.Models
{
    public class Employee
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long PhoneNumber {  get; set; }
        public string Email { get; set; }
        public string Password {  get; set; }
        public string Gender {  get; set; }
        public string Department {  get; set; }
        public string ?Profile { get; set; }
        public string? Resume { get; set; }
        public IFormFile UploadResume { get; set; }
    }
}
