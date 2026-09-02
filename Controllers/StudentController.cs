using FreshStart.Data;
using FreshStart.Models.Identity;
using FreshStart.Models.Student;
using FreshStart.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreshStart.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;


        public StudentController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
        }



        // Student Dashboard
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }


            var profile = await _context.StudentProfiles
                .FirstOrDefaultAsync(x => x.UserId == user.Id);



            var dashboard = new StudentDashboardViewModel
            {
                Profile = profile,

                ProfileCompletion = profile == null ? 20 : 70,

                TotalApplications = 0,

                SavedJobs = 0
            };


            return View(dashboard);
        }





        // Create Profile Page
        [HttpGet]
        public IActionResult CreateProfile()
        {
            return View();
        }




        // Save Profile + Resume + Image Upload
        [HttpPost]
        public async Task<IActionResult> CreateProfile(
            StudentProfile model,
            IFormFile ResumeFile,
            IFormFile ProfileImageFile)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }


            model.UserId = user.Id;
            model.User = user;


            ModelState.Remove(nameof(StudentProfile.UserId));
            ModelState.Remove(nameof(StudentProfile.User));



            // Resume Upload
            if (ResumeFile != null)
            {
                string folderPath = Path.Combine(
                    _environment.WebRootPath,
                    "uploads/resumes"
                );


                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }


                string fileName = Guid.NewGuid().ToString()
                    + Path.GetExtension(ResumeFile.FileName);


                string filePath = Path.Combine(
                    folderPath,
                    fileName
                );


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ResumeFile.CopyToAsync(stream);
                }


                model.ResumePath = "/uploads/resumes/" + fileName;
            }



            // Profile Image Upload
            if (ProfileImageFile != null)
            {
                string folderPath = Path.Combine(
                    _environment.WebRootPath,
                    "uploads/profile"
                );


                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }


                string fileName = Guid.NewGuid().ToString()
                    + Path.GetExtension(ProfileImageFile.FileName);


                string filePath = Path.Combine(
                    folderPath,
                    fileName
                );


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImageFile.CopyToAsync(stream);
                }


                model.ProfileImage = "/uploads/profile/" + fileName;
            }



            if (ModelState.IsValid)
            {
                _context.StudentProfiles.Add(model);

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }


            return View(model);
        }






        // Edit Profile Page
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }


            var profile = await _context.StudentProfiles
                .FirstOrDefaultAsync(x => x.UserId == user.Id);


            if (profile == null)
            {
                return RedirectToAction("CreateProfile");
            }


            return View(profile);
        }





        // Update Profile + Resume + Image Upload
        [HttpPost]
        public async Task<IActionResult> EditProfile(
            StudentProfile model,
            IFormFile ResumeFile,
            IFormFile ProfileImageFile)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }


            var profile = await _context.StudentProfiles
                .FirstOrDefaultAsync(x => x.UserId == user.Id);



            if (profile == null)
            {
                return RedirectToAction("CreateProfile");
            }



            profile.FullName = model.FullName;
            profile.Phone = model.Phone;
            profile.City = model.City;
            profile.University = model.University;
            profile.Degree = model.Degree;
            profile.GraduationYear = model.GraduationYear;
            profile.Skills = model.Skills;
            profile.Experience = model.Experience;
            profile.Bio = model.Bio;
            profile.LinkedIn = model.LinkedIn;
            profile.GitHub = model.GitHub;




            // New Resume Upload
            if (ResumeFile != null)
            {
                string folderPath = Path.Combine(
                    _environment.WebRootPath,
                    "uploads/resumes"
                );


                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }


                string fileName = Guid.NewGuid().ToString()
                    + Path.GetExtension(ResumeFile.FileName);


                string filePath = Path.Combine(
                    folderPath,
                    fileName
                );


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ResumeFile.CopyToAsync(stream);
                }


                profile.ResumePath = "/uploads/resumes/" + fileName;
            }





            // New Profile Image Upload
            if (ProfileImageFile != null)
            {
                string folderPath = Path.Combine(
                    _environment.WebRootPath,
                    "uploads/profile"
                );


                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }


                string fileName = Guid.NewGuid().ToString()
                    + Path.GetExtension(ProfileImageFile.FileName);


                string filePath = Path.Combine(
                    folderPath,
                    fileName
                );


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImageFile.CopyToAsync(stream);
                }


                profile.ProfileImage = "/uploads/profile/" + fileName;
            }



            await _context.SaveChangesAsync();


            return RedirectToAction("Index");
        }

    }
}