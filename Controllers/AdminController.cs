using FreshStart.Data;
using FreshStart.Models.Identity;
using FreshStart.Models.Employer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FreshStart.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;



        public AdminController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }





        // Admin Dashboard
        public async Task<IActionResult> Index()
        {

            var totalStudents = await _userManager
                .GetUsersInRoleAsync("Student");



            var totalEmployers = await _userManager
                .GetUsersInRoleAsync("Employer");



            var pendingEmployers = await _context.EmployerProfiles
                .CountAsync(x => x.ApprovalStatus == "Pending");



            var approvedEmployers = await _context.EmployerProfiles
                .CountAsync(x => x.ApprovalStatus == "Approved");





            ViewBag.TotalStudents = totalStudents.Count;

            ViewBag.TotalEmployers = totalEmployers.Count;

            ViewBag.PendingEmployers = pendingEmployers;

            ViewBag.ApprovedEmployers = approvedEmployers;



            return View();

        }






        // View All Employers
        public async Task<IActionResult> Employers()
        {

            var employers = await _context.EmployerProfiles
                .Include(x => x.User)
                .ToListAsync();



            return View(employers);

        }







        // Approve Employer
        public async Task<IActionResult> ApproveEmployer(int id)
        {

            var employer = await _context.EmployerProfiles
                .FirstOrDefaultAsync(x => x.EmployerId == id);



            if (employer == null)
            {
                return NotFound();
            }



            employer.ApprovalStatus = "Approved";


            await _context.SaveChangesAsync();



            return RedirectToAction("Employers");

        }







        // Reject Employer
        public async Task<IActionResult> RejectEmployer(int id)
        {

            var employer = await _context.EmployerProfiles
                .FirstOrDefaultAsync(x => x.EmployerId == id);



            if (employer == null)
            {
                return NotFound();
            }



            employer.ApprovalStatus = "Rejected";


            await _context.SaveChangesAsync();



            return RedirectToAction("Employers");

        }

        // View All Jobs
        public async Task<IActionResult> Jobs()
        {
            var jobs = await _context.Jobs
                .Include(x => x.Employer)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();


            return View(jobs);
        }

        // View All Students
        public async Task<IActionResult> Students()
        {
            var students = await _context.StudentProfiles
                .Include(x => x.User)
                .ToListAsync();


            return View(students);
        }


    }

}