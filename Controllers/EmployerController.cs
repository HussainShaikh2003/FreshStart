using FreshStart.Data;
using FreshStart.Models.Employer;
using FreshStart.Models.Identity;
using FreshStart.Models.Job;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FreshStart.Models.Application;
using FreshStart.Models.Notification;
using FreshStart.Models.ViewModels;
using FreshStart.Services;

namespace FreshStart.Controllers
{
    public class EmployerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly JobMatchingService _jobMatchingService;

        public EmployerController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment,
            JobMatchingService jobMatchingService)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
            _jobMatchingService = jobMatchingService;
        }







        // Employer Dashboard
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);


            if (user == null)
            {
                return Challenge();
            }



            var company = await _context.EmployerProfiles
                .FirstOrDefaultAsync(x => x.UserId == user.Id);



            return View(company);
        }








        // Create Company Profile Page
        [HttpGet]
        public IActionResult CreateProfile()
        {
            return View();
        }








        // Save Company Profile + Logo Upload
        [HttpPost]
        public async Task<IActionResult> CreateProfile(
            EmployerProfile model,
            IFormFile CompanyLogoFile)
        {
            var user = await _userManager.GetUserAsync(User);


            if (user == null)
            {
                return Challenge();
            }



            model.UserId = user.Id;
            model.User = user;



            ModelState.Remove(nameof(EmployerProfile.UserId));
            ModelState.Remove(nameof(EmployerProfile.User));




            if (CompanyLogoFile != null)
            {
                string folderPath = Path.Combine(
                    _environment.WebRootPath,
                    "uploads/company"
                );


                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }



                string fileName = Guid.NewGuid().ToString()
                    + Path.GetExtension(CompanyLogoFile.FileName);



                string filePath = Path.Combine(
                    folderPath,
                    fileName
                );



                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await CompanyLogoFile.CopyToAsync(stream);
                }



                model.CompanyLogo = "/uploads/company/" + fileName;
            }




            if (ModelState.IsValid)
            {
                _context.EmployerProfiles.Add(model);


                await _context.SaveChangesAsync();


                return RedirectToAction("Index");
            }



            return View(model);
        }









        // Edit Company Profile Page
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);


            if (user == null)
            {
                return Challenge();
            }



            var company = await _context.EmployerProfiles
                .FirstOrDefaultAsync(x => x.UserId == user.Id);



            if (company == null)
            {
                return RedirectToAction("CreateProfile");
            }



            return View(company);
        }









        // Update Company Profile + Logo Upload
        [HttpPost]
        public async Task<IActionResult> EditProfile(
            EmployerProfile model,
            IFormFile CompanyLogoFile)
        {
            var user = await _userManager.GetUserAsync(User);


            if (user == null)
            {
                return Challenge();
            }



            var company = await _context.EmployerProfiles
                .FirstOrDefaultAsync(x => x.UserId == user.Id);



            if (company == null)
            {
                return RedirectToAction("CreateProfile");
            }





            company.CompanyName = model.CompanyName;
            company.CompanyEmail = model.CompanyEmail;
            company.Phone = model.Phone;
            company.Website = model.Website;
            company.Industry = model.Industry;
            company.CompanyDescription = model.CompanyDescription;





            if (CompanyLogoFile != null)
            {
                string folderPath = Path.Combine(
                    _environment.WebRootPath,
                    "uploads/company"
                );


                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }



                string fileName = Guid.NewGuid().ToString()
                    + Path.GetExtension(CompanyLogoFile.FileName);



                string filePath = Path.Combine(
                    folderPath,
                    fileName
                );



                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await CompanyLogoFile.CopyToAsync(stream);
                }



                company.CompanyLogo = "/uploads/company/" + fileName;
            }





            await _context.SaveChangesAsync();



            return RedirectToAction("Index");
        }









        // View Employer Jobs
        public async Task<IActionResult> Jobs()
        {
            var user = await _userManager.GetUserAsync(User);


            if (user == null)
            {
                return Challenge();
            }



            var company = await _context.EmployerProfiles
                .FirstOrDefaultAsync(x => x.UserId == user.Id);



            if (company == null)
            {
                return RedirectToAction("CreateProfile");
            }



            var jobs = await _context.Jobs
                .Where(x => x.EmployerId == company.EmployerId)
                .ToListAsync();



            return View(jobs);
        }









        // Create Job Page
        [HttpGet]
        public async Task<IActionResult> CreateJob()
        {
            var user = await _userManager.GetUserAsync(User);


            if (user == null)
            {
                return Challenge();
            }



            var company = await _context.EmployerProfiles
                .FirstOrDefaultAsync(x => x.UserId == user.Id);



            if (company == null)
            {
                return RedirectToAction("CreateProfile");
            }

            if (company.ApprovalStatus != "Approved")
            {
                return RedirectToAction("Index");
            }


            // Check Admin Approval

            if (company.ApprovalStatus != "Approved")
            {
                TempData["Error"] = "Your company profile is waiting for admin approval.";
                return RedirectToAction("Index");
            }




            return View();
        }








        // Save Job
        // Save Job

        [HttpPost]
        public async Task<IActionResult> CreateJob(Job model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var company = await _context.EmployerProfiles
                .FirstOrDefaultAsync(x => x.UserId == user.Id);

            if (company == null)
            {
                return RedirectToAction("CreateProfile");
            }

            // Only approved employers can post jobs
            if (company.ApprovalStatus != "Approved")
            {
                TempData["Error"] = "Your company profile is waiting for admin approval.";
                return RedirectToAction("Index");
            }

            // Salary validation
            if (model.MinimumSalary < 0 || model.MaximumSalary < 0)
            {
                ModelState.AddModelError(
                    nameof(model.MinimumSalary),
                    "Salary cannot be negative.");
            }

            if (model.MaximumSalary < model.MinimumSalary)
            {
                ModelState.AddModelError(
                    nameof(model.MaximumSalary),
                    "Maximum salary must be greater than or equal to minimum salary.");
            }

            if (string.IsNullOrWhiteSpace(model.SalaryType) ||
                !new[] { "Monthly", "Yearly", "Hourly" }
                    .Contains(model.SalaryType))
            {
                ModelState.AddModelError(
                    nameof(model.SalaryType),
                    "Please select a valid salary type.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.EmployerId = company.EmployerId;
            model.Employer = company;

            _context.Jobs.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Jobs");
        }


        // View Applicants For Job
        public async Task<IActionResult> Applicants(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var company = await _context.EmployerProfiles
                .FirstOrDefaultAsync(x => x.UserId == user.Id);

            if (company == null)
            {
                return RedirectToAction("CreateProfile");
            }

            var job = await _context.Jobs
                .FirstOrDefaultAsync(x =>
                    x.JobId == id &&
                    x.EmployerId == company.EmployerId);

            if (job == null)
            {
                return NotFound();
            }

            var applications = await _context.JobApplications
                .Include(x => x.Student)
                .Include(x => x.Job)
                .Where(x => x.JobId == id)
                .ToListAsync();

            var results = new List<ApplicantMatchViewModel>();

            foreach (var application in applications)
            {
                string resumeText = application.Student?.ResumeText ?? "";

                // Extract text only if it is not already saved
                if (string.IsNullOrWhiteSpace(resumeText) &&
                    !string.IsNullOrWhiteSpace(application.Student?.ResumePath))
                {
                    var relativePath = application.Student.ResumePath
                        .TrimStart('/')
                        .Replace('/', Path.DirectorySeparatorChar);

                    var filePath = Path.Combine(
                        _environment.WebRootPath,
                        relativePath);

                    if (System.IO.File.Exists(filePath))
                    {
                        try
                        {
                            var extractor = new ResumeTextExtractor();

                            resumeText = extractor.ExtractText(filePath);

                            // Save extracted text for future use
                            application.Student.ResumeText = resumeText;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(
                                "Resume extraction failed: " + ex.Message);

                            resumeText = "";
                        }
                    }
                }

                var match = _jobMatchingService.CalculateMatch(
                    resumeText,
                    application.Student?.Skills ?? "",
                    job.Description ?? "",
                    job.RequiredSkills ?? "");

                results.Add(new ApplicantMatchViewModel
                {
                    Application = application,
                    MatchPercentage = match.MatchPercentage,
                    MatchedTerms = match.MatchedTerms,
                    MissingTerms = match.MissingTerms
                });
            }

            // Save newly extracted resume text in database
            await _context.SaveChangesAsync();

            results = results
                .OrderByDescending(x => x.MatchPercentage)
                .ToList();

            return View(results);
        }

        // Update Application Status

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateApplicationStatus(
            int applicationId,
            string status)
        {
            var allowedStatuses = new[]
            {
        "Reviewed", "Shortlisted", "Selected", "Rejected"
    };

            if (string.IsNullOrWhiteSpace(status) ||
                !allowedStatuses.Contains(status))
            {
                return BadRequest();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            var company = await _context.EmployerProfiles
                .FirstOrDefaultAsync(x => x.UserId == user.Id);

            if (company == null)
                return Forbid();

            var application = await _context.JobApplications
                .Include(x => x.Job)
                .FirstOrDefaultAsync(x =>
                    x.ApplicationId == applicationId &&
                    x.Job.EmployerId == company.EmployerId);

            if (application == null)
                return NotFound();

            application.Status = status;

            if (status == "Selected")
                application.EmployerNotes = "Candidate Selected";
            else if (status == "Rejected")
                application.EmployerNotes = "Application Rejected";

            var student = await _context.StudentProfiles
                .FirstOrDefaultAsync(x => x.StudentId == application.StudentId);

            if (student != null)
            {
                _context.Notifications.Add(new Notification
                {
                    UserId = student.UserId,
                    Message = $"Your application status for {application.Job.JobTitle} is now {status}",
                    CreatedDate = DateTime.Now
                });
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Applicants", new { id = application.JobId });
        }

        // Public Company Profile
        public async Task<IActionResult> CompanyProfile(int id)
        {
            var company = await _context.EmployerProfiles
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.EmployerId == id);


            if (company == null)
            {
                return NotFound();
            }


            return View(company);
        }
        // Schedule Interview Page
        [HttpGet]
        public async Task<IActionResult> ScheduleInterview(int id)
        {
            var application = await _context.JobApplications
                .Include(x => x.Student)
                .Include(x => x.Job)
                .FirstOrDefaultAsync(x => x.ApplicationId == id);


            if (application == null)
            {
                return NotFound();
            }


            return View(application);
        }
        [HttpPost]
        public async Task<IActionResult> ScheduleInterview(
    int applicationId,
    DateTime interviewDate,
    string interviewTime,
    string interviewLocation)
        {
            var application = await _context.JobApplications
                .FirstOrDefaultAsync(x => x.ApplicationId == applicationId);


            if (application == null)
            {
                return NotFound();
            }


            application.Status = "Interview Scheduled";

            application.InterviewDate = interviewDate;

            application.InterviewTime = interviewTime;

            application.InterviewLocation = interviewLocation;
            var student = await _context.StudentProfiles
    .FirstOrDefaultAsync(x => x.StudentId == application.StudentId);

            if (student != null)
            {
                var notification = new Notification
                {
                    UserId = student.UserId,
                    Message = "Your interview has been scheduled.",
                    CreatedDate = DateTime.Now
                };

                _context.Notifications.Add(notification);
            }


            await _context.SaveChangesAsync();


            return RedirectToAction(
                "Applicants",
                new { id = application.JobId }
            );
        }
    }

}