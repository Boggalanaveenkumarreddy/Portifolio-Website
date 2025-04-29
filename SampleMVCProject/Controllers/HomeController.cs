using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Text;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SampleMVCProject.DbContexts;
using SampleMVCProject.Models;
using SampleMVCProject.Repositories;

namespace SampleMVCProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly EmployeeDbContext _employeeDbContext;
        public readonly IConfiguration _configuration;
        public readonly IdentityUser _identityUser;
        public HomeController(ILogger<HomeController> logger, EmployeeDbContext employeeDbContext, IConfiguration configuration, IdentityUser identityUserRepository)
        {
            _logger = logger;
            _employeeDbContext = employeeDbContext;
            _configuration = configuration;
            _identityUser = identityUserRepository;
        }
        public IActionResult Home()
        {
            //EmployeeLoginDeatails employeeLoginDeatails = new EmployeeLoginDeatails();
            //employeeLoginDeatails.Username = "boggalanaveen321@gmail.com";
            //employeeLoginDeatails.Password = "naveen@321";
            //var result = _employeeDbContext.EmployeeLoginCheck(employeeLoginDeatails);
            //HttpContext.Session.SetString("LogginedUser", JsonConvert.SerializeObject(result));
            var json = HttpContext.Session.GetString("LogginedUser");
            if (json != null)
            {
                var employee = JsonConvert.DeserializeObject<Employee>(json);
                var skills = _employeeDbContext.GetEmployeeSkills(employee.Id);
                var experiences = _employeeDbContext.GetEmployeeExperience(employee.Id);
                var viewModel = new EmployeeSkill
                {
                    Employee = employee,
                    Skills = (List<SkillDetails>)skills,
                    Experience = (List<Experience>)experiences
                };

                return View(viewModel);
            }
            return RedirectToAction("EmployeeLogin");
        }

        [HttpPost]
        public IActionResult Home(EmployeeSkill viewModel)
        {
            var json = HttpContext.Session.GetString("LogginedUser");
            if (json != null)
            {
                var employee = JsonConvert.DeserializeObject<Employee>(json);

                if (!ModelState.IsValid)
                {
                    viewModel.Employee = employee;
                    viewModel.Skills = _employeeDbContext.GetEmployeeSkills(employee.Id).ToList();
                    viewModel.Experience = _employeeDbContext.GetEmployeeExperience(employee.Id).ToList();
                    return View(viewModel);
                }
                _employeeDbContext.CreateContactUs(viewModel.ContactUs);
                StringBuilder mailBody = new StringBuilder();
                mailBody.AppendLine("<h3>Contact Us Form Submission</h3>");
                mailBody.AppendLine("<p><strong>Full Name:</strong> " + viewModel.ContactUs.FullName + "</p>");
                mailBody.AppendLine("<p><strong>Phone Number:</strong> " + viewModel.ContactUs.PhoneNumber + "</p>");
                mailBody.AppendLine("<p><strong>Email:</strong> " + viewModel.ContactUs.Email + "</p>");
                mailBody.AppendLine("<p><strong>Message:</strong></p>");
                mailBody.AppendLine("<p><i>" + viewModel.ContactUs.Message + "</i></p>");
                mailBody.AppendLine("<p>Thank you for contacting us. We will get back to you shortly.</p>");
                mailBody.AppendLine("<p>Best regards,</p>");
                mailBody.AppendLine("<p>Naveen Kumar</p>");
                using (MailMessage mail = new MailMessage())
                {
                    string Displayname = _configuration["AppSettings:DisplayName"];
                    string mailFrom = _configuration["AppSettings:smtpUser"];
                    mail.To.Add(viewModel.ContactUs.Email.Trim());
                    mail.From = new MailAddress(mailFrom, Displayname);
                    mail.Subject = "Contacting me";
                    mail.Body = mailBody.ToString();
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = _configuration["AppSettings:smtpServer"];
                        smtp.Port = Convert.ToInt32(_configuration["AppSettings:smtpPort"]);
                        smtp.EnableSsl = Convert.ToBoolean(_configuration["AppSettings:EnableSsl"]);
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(
                            _configuration["AppSettings:smtpUser"],
                            _configuration["AppSettings:PWD"]);
                        smtp.Timeout = 20000;
                        smtp.Send(mail);
                    }
                }

                TempData["SuccessMessage"] = "Form Submitted Successfully";
                return Redirect("/Home/Home#contactUs");
            }
            return RedirectToAction("EmployeeLogin");
        }
        public IActionResult RegisterNewUser()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterNewUser(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }
            var result = _employeeDbContext.CreateEmployee(employee);
            if (result == null)
            {
                TempData["ErrorMessage"] = "Error Ocurred  when Creating Account";
            }
            else
            {
                TempData["SuccessMessage"] = "User Registered successfully";
            }
            return View();

        }

        public IActionResult Index()
        {
            var result = _employeeDbContext.GetAllEmployees();
            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult EditProfileData()
        {
            var json = HttpContext.Session.GetString("LogginedUser");
            Employee result = null;
            if (json != null)
            {
                var employee1 = JsonConvert.DeserializeObject<Employee>(json);
                result = _employeeDbContext.GetEmployee(employee1.Id);
            }
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> EditProfileData(Employee employee)
        {
            var json = HttpContext.Session.GetString("LogginedUser");
            if (json != null)
            {
                var employee1 = JsonConvert.DeserializeObject<Employee>(json);
                employee.Id = employee1.Id;
            }
            if (employee.UploadResume != null && employee.UploadResume.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(employee.UploadResume.FileName);
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ImageUpload", "ResumeUploads");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await employee.UploadResume.CopyToAsync(stream);
                }
                employee.Resume = Path.Combine("ImageUpload", "ResumeUploads", fileName).Replace("\\", "/");
            }
            else
            {
                employee.Resume = _employeeDbContext.Get_Resume_By_Id(employee.Id);
            }
            _employeeDbContext.UpdateProfileData(employee);
            TempData["SuccessMessage"] = "Profile Data Updated Successfully";
            return RedirectToAction("Index");
        }

        public IActionResult EditEmployee(int id)
        {
            var result = _employeeDbContext.GetEmployee(id);
            return View(result);
        }
        [HttpPost]
        public IActionResult EditEmployee(Employee employee)
        {
            _employeeDbContext.UpdateEmployee(employee);
            TempData["SuccessMessage"] = "Employee Data Updated Successfully";
            return RedirectToAction("Index");
        }
        public IActionResult CreateEmployee()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateEmployee(Employee employee)
        {
            var result = _employeeDbContext.CreateEmployee(employee);
            if (result == null)
            {
                TempData["ErrorMessage"] = "Error Ocurred  when employee adding";
            }
            else
            {
                TempData["SuccessMessage"] = "Employee Created successfully";
            }
            return View();
        }
        public IActionResult DeleteEmployee(int id)
        {
            _employeeDbContext.DeleteEmployee(id);
            TempData["SuccessMessage"] = "Employee deleted Successfully";
            return RedirectToAction("Index");
        }
        public IActionResult UpdatePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UpdatePassword(EmployeePasswordUpdate employeePasswordUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View(employeePasswordUpdate);
            }
            var json = HttpContext.Session.GetString("LogginedUser");
            if (json != null)
            {
                var employee1 = JsonConvert.DeserializeObject<Employee>(json);
                employeePasswordUpdate.Id = employee1.Id;
            }

            _employeeDbContext.EmployeePasswordUpdate(employeePasswordUpdate);
            TempData["SuccessMessage"] = "Password Updated Successfully";
            return View();
        }
        public IActionResult EmployeeLogin()
        {
            HttpContext.Session.Clear();
            return View();
        }
        [HttpPost]
        public IActionResult EmployeeLogin(EmployeeLoginDeatails employeeLoginDeatails)
        {
            if (!ModelState.IsValid)
            {
                return View(employeeLoginDeatails);
            }
            var result = _employeeDbContext.EmployeeLoginCheck(employeeLoginDeatails);
            if (result == null)
            {
                TempData["ErrorMessage"] = "Invalid Credentials";
            }
            if (result != null)
            {
                TempData["SuccessMessage"] = "Logined Successfully";
                HttpContext.Session.SetString("LogginedUser", JsonConvert.SerializeObject(result));
                return RedirectToAction("Home");
            }
            return View();
        }
        public IActionResult UploadProfilePicture()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(ProfileUpload employee)
        {
            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            if (employee.UploadProfile != null && employee.UploadProfile.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(employee.UploadProfile.FileName);
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ImageUpload", "ProfilePictures");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await employee.UploadProfile.CopyToAsync(stream);
                }
                employee.Profile = Path.Combine("ImageUpload", "ProfilePictures", fileName).Replace("\\", "/");
            }

            var json = HttpContext.Session.GetString("LogginedUser");
            if (json != null)
            {
                var employee1 = JsonConvert.DeserializeObject<Employee>(json);
                employee.Id = employee1.Id;
            }
            _employeeDbContext.UploadProfilePicture(employee);
            TempData["SuccessMessage"] = "Uploaded Successfully";
            return View();
        }

        public IActionResult AddSkill()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddSkill(SkillDetails skillDetails)
        {
            if (!ModelState.IsValid)
            {
                return View(skillDetails);
            }
            var json = HttpContext.Session.GetString("LogginedUser");
            if (json != null)
            {
                var employee1 = JsonConvert.DeserializeObject<Employee>(json);
                skillDetails.EmployeeId = employee1.Id;
            }
            if (skillDetails.UploadImage != null && skillDetails.UploadImage.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(skillDetails.UploadImage.FileName);
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ImageUpload", "SkillImages");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await skillDetails.UploadImage.CopyToAsync(stream);
                }
                skillDetails.SkillImage = Path.Combine("ImageUpload", "SkillImages", fileName).Replace("\\", "/");
            }
            _employeeDbContext.AddSkill(skillDetails);
            TempData["SuccessMessage"] = "Skill Added Successfully";
            return View();
        }
        public IActionResult AddExperience()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddExperience(Experience experience)
        {
            if (!ModelState.IsValid)
            {
                return View(experience);
            }
            var json = HttpContext.Session.GetString("LogginedUser");
            if (json != null)
            {
                var employee = JsonConvert.DeserializeObject<Employee>(json);
                experience.EmployeeId = employee.Id;
            }
            _employeeDbContext.AddExperience(experience);
            TempData["SuccessMessage"] = "Experience Added Successfully";
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPassword User)
        {

            string password = _employeeDbContext.GetPassword_By_Username(User.Username);
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendLine("<p>Your Password is " + password + "</p>");
            using (MailMessage mail = new MailMessage())
            {
                string Displayname = _configuration["AppSettings:DisplayName"];
                string mailFrom = _configuration["AppSettings:smtpUser"];
                mail.To.Add(User.Username.Trim());
                mail.From = new MailAddress(mailFrom, Displayname);
                mail.Subject = "Contacting me";
                mail.Body = mailBody.ToString();
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = _configuration["AppSettings:smtpServer"];
                    smtp.Port = Convert.ToInt32(_configuration["AppSettings:smtpPort"]);
                    smtp.EnableSsl = Convert.ToBoolean(_configuration["AppSettings:EnableSsl"]);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(
                        _configuration["AppSettings:smtpUser"],
                        _configuration["AppSettings:PWD"]);
                    smtp.Timeout = 20000;
                    smtp.Send(mail);
                }
            }
            TempData["SuccessMessage"] = "Password is sent to your Email";
            return View();
        }

    }
}
