using FreshStart.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FreshStart.Models.Student;
using FreshStart.Models.Employer;

namespace FreshStart.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
           
    }
        public DbSet<StudentProfile> StudentProfiles { get; set; }

        public DbSet<EmployerProfile> EmployerProfiles { get; set; }
    }
}