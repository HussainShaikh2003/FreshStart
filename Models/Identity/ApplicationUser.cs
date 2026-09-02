using Microsoft.AspNetCore.Identity;

namespace FreshStart.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}