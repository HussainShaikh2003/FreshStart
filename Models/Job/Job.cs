using FreshStart.Models.Employer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreshStart.Models.Job
{
    public class Job
    {

        [Key]
        public int JobId { get; set; }



        // Employer Foreign Key
        [Required]
        public int EmployerId { get; set; }



        [ForeignKey("EmployerId")]
        public EmployerProfile? Employer { get; set; }




        [Required]
        public string JobTitle { get; set; }




        [Required]
        public string Description { get; set; }




        public string RequiredSkills { get; set; }




        public string Location { get; set; }




        public string Salary { get; set; }




        public string JobType { get; set; }




        public DateTime Deadline { get; set; }




        public DateTime CreatedDate { get; set; } = DateTime.Now;


    }
}