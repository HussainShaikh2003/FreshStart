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

            var totalStudents = await _context.StudentProfiles
     .CountAsync();


            var totalEmployers = await _context.EmployerProfiles
                .CountAsync();



            var pendingEmployers = await _context.EmployerProfiles
                .CountAsync(x => x.ApprovalStatus == "Pending");



            var approvedEmployers = await _context.EmployerProfiles
                .CountAsync(x => x.ApprovalStatus == "Approved");





            ViewBag.TotalStudents = totalStudents;

            ViewBag.TotalEmployers = totalEmployers;

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

        // Delete Employer
        public async Task<IActionResult> DeleteEmployer(int id)
        {
            var employer = await _context.EmployerProfiles
                .FirstOrDefaultAsync(x => x.EmployerId == id);


            if (employer == null)
            {
                return NotFound();
            }


            // Get employer jobs
            var jobs = await _context.Jobs
                .Where(x => x.EmployerId == id)
                .ToListAsync();



            foreach (var job in jobs)
            {
                var applications = await _context.JobApplications
                    .Where(x => x.JobId == job.JobId)
                    .ToListAsync();


                _context.JobApplications.RemoveRange(applications);
            }



            // Delete Jobs
            _context.Jobs.RemoveRange(jobs);



            // Delete Employer
            _context.EmployerProfiles.Remove(employer);



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
        // Delete Job
        public async Task<IActionResult> DeleteJob(int id)
        {
            var job = await _context.Jobs
                .FirstOrDefaultAsync(x => x.JobId == id);


            if (job == null)
            {
                return NotFound();
            }



            var applications = await _context.JobApplications
                .Where(x => x.JobId == id)
                .ToListAsync();



            _context.JobApplications.RemoveRange(applications);



            _context.Jobs.Remove(job);



            await _context.SaveChangesAsync();



            return RedirectToAction("Jobs");
        }
        // Delete Student
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.StudentProfiles
                .FirstOrDefaultAsync(x => x.StudentId == id);


            if (student == null)
            {
                return NotFound();
            }



            // Delete Student Applications first
            var applications = await _context.JobApplications
                .Where(x => x.StudentId == id)
                .ToListAsync();


            _context.JobApplications.RemoveRange(applications);



            // Get Student User
            var user = await _userManager.FindByIdAsync(student.UserId);



            // Delete Student Profile
            _context.StudentProfiles.Remove(student);



            // Delete Identity User
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }



            await _context.SaveChangesAsync();


            return RedirectToAction("Students");
        }
    }

}