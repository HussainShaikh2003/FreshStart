using Microsoft.AspNetCore.Identity;
using FreshStart.Models.Identity;

namespace FreshStart.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRoles(
            RoleManager<IdentityRole> roleManager)
        {
            string[] roles =
            {
                "Admin",
                "Student",
                "Employer"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(role));
                }
            }
        }


        public static async Task SeedAdmin(
            UserManager<ApplicationUser> userManager)
        {
            string email = "admin@freshstart.com";
            string password = "Admin@123";


            var user = await userManager.FindByEmailAsync(email);


            if (user == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    Role = "Admin"
                };


                var result = await userManager.CreateAsync(
                    admin,
                    password);


                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(
                        admin,
                        "Admin");
                }
            }
        }
    }
}