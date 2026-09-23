using FreshStart.Models.Identity;
using FreshStart.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FreshStart.Controllers
{
    public class AdminAuthController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public AdminAuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }




        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }





        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }



            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                false,
                false);



            if (result.Succeeded)
            {

                var user = await _userManager
                    .FindByEmailAsync(model.Email);



                var roles = await _userManager
                    .GetRolesAsync(user);



                if (roles.Contains("Admin"))
                {
                    return RedirectToAction(
                        "Index",
                        "Admin");
                }

            }



            ModelState.AddModelError(
                "",
                "Invalid admin login");


            return View(model);

        }

    }
}