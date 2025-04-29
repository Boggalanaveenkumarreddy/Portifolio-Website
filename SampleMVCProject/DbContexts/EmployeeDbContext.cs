using System.Data;
using Dapper;
using SampleMVCProject.Models;

namespace SampleMVCProject.DbContexts
{
    public class EmployeeDbContext
    {
        private readonly IDbConnection _db;
        public EmployeeDbContext(IDbConnection db)
        {
            _db = db;
        }
        public IEnumerable<Employee> GetAllEmployees()
        {
            return _db.Query<Employee>("Get_All_Employees", commandType: CommandType.StoredProcedure, commandTimeout: 600);
        }
        public Employee GetEmployee(long id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("Id", id);
            return _db.Query<Employee>("Get_Employee", param, commandType: CommandType.StoredProcedure, commandTimeout: 600).FirstOrDefault();
        }
        public void UpdateEmployee(Employee employee)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("Id", employee.Id);
            param.Add("Name", employee.Name);
            param.Add("PhoneNumber", employee.PhoneNumber);
            param.Add("Email", employee.Email);
            param.Add("Gender", employee.Gender);
            param.Add("Department", employee.Department);
            _db.Execute("Save_Employee", param, commandType: CommandType.StoredProcedure, commandTimeout: 600);
        }
        public void DeleteEmployee(int id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("Id", id);
            _db.Query("Delete_Employee", param, commandType: CommandType.StoredProcedure, commandTimeout: 600);
        }
        public string CreateEmployee(Employee employee)
        {
            DynamicParameters param = new DynamicParameters();
            //param.Add("Id", employee.Id);
            //int count = _db.ExecuteScalar<int>("Check_Employee_Exist", param, commandType: CommandType.StoredProcedure, commandTimeout: 600);
            //if (count > 0)
            //{
            //    return "Employee Id already exist";
            //}
            param.Add("Name", employee.Name);
            param.Add("PhoneNumber", employee.PhoneNumber);
            param.Add("Email", employee.Email);
            param.Add("Gender", employee.Gender);
            param.Add("Department", employee.Department);
            param.Add("Password", employee.Password);
            _db.Query("Create_New_Employee", param, commandType: CommandType.StoredProcedure, commandTimeout: 600);
            return "Employee Created Successfully";
        }
        public void EmployeePasswordUpdate(EmployeePasswordUpdate employee)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("NewPassword", employee.NewPassword);
            param.Add("Id", employee.Id);
            _db.Query("UpdatePassword", param, commandType: CommandType.StoredProcedure, commandTimeout: 600);
        }
        public Employee EmployeeLoginCheck(EmployeeLoginDeatails employeeLoginDeatails)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("Username", employeeLoginDeatails.Username);
            param.Add("Password", employeeLoginDeatails.Password);
            return _db.Query<Employee>("Get_Employee_With_UserName_Pwd", param,commandType:CommandType.StoredProcedure,commandTimeout:600).FirstOrDefault();
        }
        public void UploadProfilePicture(ProfileUpload employee)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("Id", employee.Id);
            param.Add("Profile", employee.Profile);
            _db.Query("UploadProfilePicture", param, commandType: CommandType.StoredProcedure, commandTimeout: 600);
        }
        public void UpdateProfileData(Employee employee)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("Id",employee.Id );
            param.Add("Name", employee.Name);
            param.Add("PhoneNumber",employee.PhoneNumber);
            param.Add("Department",employee.Department);
            param.Add("Gender",employee.Gender);
            param.Add("Email",employee.Email);
            param.Add("Resume", employee.Resume);
            _db.Query("Update_Profile_Data", param, commandType: CommandType.StoredProcedure, commandTimeout: 600);
        }
        public void CreateContactUs(ContactUs contactUs)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("FullName",contactUs.FullName);
            param.Add("Email",contactUs.Email);
            param.Add("PhoneNumber",contactUs.PhoneNumber);
            param.Add("Message",contactUs.Message);
            _db.Query("Create_ContactUs_Application",param, commandType: CommandType.StoredProcedure,commandTimeout: 600);
        }
        public void AddSkill(SkillDetails skillDetails)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("EmployeeId",skillDetails.EmployeeId);
            param.Add("SkillName", skillDetails.SkillName );
            param.Add("SkillDescription", skillDetails.SkillDescription );
            param.Add("SkillImage", skillDetails.SkillImage );
            _db.Query("Add_New_Skill", param, commandType: CommandType.StoredProcedure, commandTimeout: 600);
        }
        public IEnumerable<SkillDetails> GetEmployeeSkills(long employeeId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("EmployeeId", employeeId);
            return _db.Query<SkillDetails>("Get_Skills_Of_Employee", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 600);
        }
        public  void AddExperience(Experience experience)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("EmployeeId",experience.EmployeeId);
            parameters.Add("CompanyName",experience.CompanyName);
            parameters.Add("Duration",experience.Duration);
            parameters.Add("ExperienceDescription",experience.ExperienceDescription);
            _db.Execute("Add_New_Experience", parameters,commandType:CommandType.StoredProcedure,commandTimeout: 600);
        }
        public IEnumerable<Experience> GetEmployeeExperience(long employeeId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("EmployeeId",employeeId);
            return _db.Query<Experience>("Get_ExperienceDetails", parameters, commandType: CommandType.StoredProcedure, commandTimeout: 600);
        }
         public string GetPassword_By_Username(string Username)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("Username", Username);
            return _db.Query<string>("GetPassword_By_Username",param,commandType:CommandType.StoredProcedure,commandTimeout:600).FirstOrDefault();
        }
        public string Get_Resume_By_Id(long id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("Id", id);
            return _db.Query<string>("Get_Resume_By_Id", param, commandType: CommandType.StoredProcedure, commandTimeout: 600).FirstOrDefault();
        }
    }
}
