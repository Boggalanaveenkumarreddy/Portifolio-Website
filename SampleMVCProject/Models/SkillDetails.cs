namespace SampleMVCProject.Models
{
    public class SkillDetails
    {
        public long EmployeeId { get; set; }
        public long SkillId { get; set; }
        public string SkillName { get; set; }
        public string SkillDescription { get; set; }
        public string? SkillImage { get; set; }
        public   IFormFile UploadImage { get; set; }
    }
}
