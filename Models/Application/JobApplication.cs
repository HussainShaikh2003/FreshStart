using FreshStart.Models.Student;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreshStart.Models.Application
{
    public class JobApplication
    {

        [Key]
        public int ApplicationId { get; set; }



        [Required]
        public int JobId { get; set; }



        [ForeignKey("JobId")]
        public FreshStart.Models.Job.Job Job { get; set; }



        [Required]
        public int StudentId { get; set; }



        [ForeignKey("StudentId")]
        public StudentProfile Student { get; set; }



        public DateTime AppliedDate { get; set; } = DateTime.Now;



        public string Status { get; set; } = "Pending";

    }
}