using FreshStart.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreshStart.Models.Student
{
    public class StudentProfile
    {
        [Key]
        public int StudentId { get; set; }


        [Required]
        public string UserId { get; set; }


        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }



        [Required]
        public string FullName { get; set; }



        public string? Phone { get; set; }


        public string? City { get; set; }



        public string? University { get; set; }


        public string? Degree { get; set; }


        public string? GraduationYear { get; set; }



        public string? Skills { get; set; }


        public string? Experience { get; set; }


        public string? Bio { get; set; }



        public string? ResumePath { get; set; }

        public string? ResumeText { get; set; }

        public string? ProfileImage { get; set; }



        public string? LinkedIn { get; set; }


        public string? GitHub { get; set; }

    }
}