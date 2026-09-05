using FreshStart.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FreshStart.Models.Employer
{
    public class EmployerProfile
    {
        [Key]
        public int EmployerId { get; set; }



        [Required]
        public string UserId { get; set; }



        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }



        [Required]
        public string CompanyName { get; set; }



        public string? CompanyEmail { get; set; }


        public string? Phone { get; set; }


        public string? Website { get; set; }


        public string? Industry { get; set; }



        public string? CompanyDescription { get; set; }



        public string? CompanyLogo { get; set; }



        // Pending / Approved / Rejected
        public string ApprovalStatus { get; set; } = "Pending";



        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}