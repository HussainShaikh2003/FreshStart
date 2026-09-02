using FreshStart.Models.Student;

namespace FreshStart.Models.ViewModels
{
    public class StudentDashboardViewModel
    {
        public StudentProfile Profile { get; set; }

        public int ProfileCompletion { get; set; }

        public int TotalApplications { get; set; }

        public int SavedJobs { get; set; }
    }
}