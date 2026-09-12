using FreshStart.Models.Job;
using FreshStart.Models.Student;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FreshStart.Models.Saved;

namespace FreshStart.Models.Saved
{
    public class SavedJob
    {

        [Key]
        public int SavedJobId { get; set; }



        [Required]
        public int StudentId { get; set; }


        [ForeignKey("StudentId")]
        public StudentProfile Student { get; set; }



        [Required]
        public int JobId { get; set; }


        [ForeignKey("JobId")]
        public FreshStart.Models.Job.Job Job { get; set; }



        public DateTime SavedDate { get; set; } = DateTime.Now;


    }
}