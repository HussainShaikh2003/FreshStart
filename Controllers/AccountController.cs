using FreshStart.Models.Identity;
using FreshStart.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FreshStart.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;


        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        // Register Page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        // Register Submit
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true,
                Role = model.Role,
                IsActive = true,
                CreatedAt = DateTime.Now
            };


            var result = await _userManager.CreateAsync(
                user,
                model.Password);


            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(
                    user,
                    model.Role);


                await _signInManager.SignInAsync(
                    user,
                    false);


                return RedirectToAction(
                    "Index",
                    "Home");
            }


            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(
                    "",
                    error.Description);
            }


            return View(model);
        }



        // Login Page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }



        // Login Submit
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
                model.RememberMe,
                false);



            if (result.Succeeded)
            {
                return RedirectToAction(
                    "Index",
                    "Home");
            }



            ModelState.AddModelError(
                "",
                "Invalid email or password");


            return View(model);
        }
    }
}