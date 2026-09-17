using FreshStart.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FreshStart.Models.Student;
using FreshStart.Models.Employer;
using FreshStart.Models.Job;
using FreshStart.Models.Application;
using FreshStart.Models.Saved;
using FreshStart.Models.Notification;
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

        public DbSet<Job> Jobs { get; set; }

        public DbSet<JobApplication> JobApplications { get; set; }

        public DbSet<SavedJob> SavedJobs { get; set; }

        public DbSet<Notification> Notifications { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<JobApplication>()
                .HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<JobApplication>()
                .HasOne(x => x.Job)
                .WithMany()
                .HasForeignKey(x => x.JobId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<SavedJob>()
    .HasOne(x => x.Student)
    .WithMany()
    .HasForeignKey(x => x.StudentId)
    .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<SavedJob>()
                .HasOne(x => x.Job)
                .WithMany()
                .HasForeignKey(x => x.JobId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}