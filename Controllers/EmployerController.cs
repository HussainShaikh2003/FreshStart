using FreshStart.Data;
using FreshStart.Models.Employer;
using FreshStart.Models.Identity;
using FreshStart.Models.Job;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FreshStart.Models.Application;

namespace FreshStart.Controllers
{
    public class EmployerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;



        public EmployerController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
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



            model.EmployerId = company.EmployerId;

            model.Employer = company;



            if (ModelState.IsValid)
            {
                _context.Jobs.Add(model);


                await _context.SaveChangesAsync();


                return RedirectToAction("Jobs");
            }



            return View(model);
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
                .FirstOrDefaultAsync(x => x.JobId == id
                                       && x.EmployerId == company.EmployerId);



            if (job == null)
            {
                return NotFound();
            }





            var applications = await _context.JobApplications
                .Include(x => x.Student)
                .Include(x => x.Job)
                .Where(x => x.JobId == id)
                .ToListAsync();



            return View(applications);
        }
        // Update Application Status
        [HttpPost]
        public async Task<IActionResult> UpdateApplicationStatus(
            int applicationId,
            string status)
        {
            var application = await _context.JobApplications
                .FirstOrDefaultAsync(x => x.ApplicationId == applicationId);


            if (application == null)
            {
                return NotFound();
            }



            application.Status = status;


            await _context.SaveChangesAsync();



            return RedirectToAction(
                "Applicants",
                new { id = application.JobId }
            );
        }
    }

}