using FreshStart.Data;
using FreshStart.Models.Employer;
using FreshStart.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

    }
}