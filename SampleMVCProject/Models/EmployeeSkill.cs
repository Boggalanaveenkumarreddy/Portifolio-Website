using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SampleMVCProject.Models
{
    public class EmployeeSkill
    {
        [ValidateNever]
        public Employee Employee { get; set; }
        [ValidateNever]
        public List<SkillDetails> Skills { get; set; }
        [ValidateNever]
        public List<Experience> Experience {  get; set; }
        public ContactUs ContactUs { get; set; }
    }
}
