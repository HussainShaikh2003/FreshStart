using FreshStart.Data;
using FreshStart.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreshStart.Controllers
{
    public class NotificationController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;



        public NotificationController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }





        // View Notifications
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);


            if (user == null)
            {
                return Challenge();
            }



            var notifications = await _context.Notifications
                .Where(x => x.UserId == user.Id)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();





            // Mark notifications as read

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }



            await _context.SaveChangesAsync();




            return View(notifications);
        }


    }
}