using FreshStart.Data;
using FreshStart.Models.Identity;
using FreshStart.Models.Student;
using FreshStart.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FreshStart.Models.Application;

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



            int totalApplications = 0;


            if (profile != null)
            {
                totalApplications = await _context.JobApplications
                    .CountAsync(x => x.StudentId == profile.StudentId);
            }



            var dashboard = new StudentDashboardViewModel
            {
                Profile = profile,

                ProfileCompletion = profile == null ? 20 : 70,


                TotalApplications = totalApplications,


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
        // Available Jobs
        public async Task<IActionResult> Jobs()
        {
            var jobs = await _context.Jobs
                .Include(x => x.Employer)
                .ToListAsync();


            return View(jobs);
        }


        // Job Details
        public async Task<IActionResult> JobDetails(int id)
        {
            var job = await _context.Jobs
                .Include(x => x.Employer)
                .FirstOrDefaultAsync(x => x.JobId == id);


            if (job == null)
            {
                return NotFound();
            }


            return View(job);
        }
        // Apply Job
        [HttpPost]
        public async Task<IActionResult> ApplyJob(int id)
        {
            var user = await _userManager.GetUserAsync(User);


            if (user == null)
            {
                return Challenge();
            }



            var student = await _context.StudentProfiles
                .FirstOrDefaultAsync(x => x.UserId == user.Id);



            if (student == null)
            {
                return RedirectToAction("CreateProfile");
            }



            var job = await _context.Jobs
                .FirstOrDefaultAsync(x => x.JobId == id);



            if (job == null)
            {
                return NotFound();
            }



            var alreadyApplied = await _context.JobApplications
                .AnyAsync(x => x.JobId == id
                            && x.StudentId == student.StudentId);



            if (alreadyApplied)
            {
                return RedirectToAction("JobDetails", new { id = id });
            }



            var application = new JobApplication
            {
                JobId = job.JobId,

                StudentId = student.StudentId,

                AppliedDate = DateTime.Now,

                Status = "Pending"
            };



            _context.JobApplications.Add(application);


            await _context.SaveChangesAsync();



            return RedirectToAction("JobDetails", new { id = id });
        }

    }
}