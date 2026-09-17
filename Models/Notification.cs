using System.ComponentModel.DataAnnotations;

namespace FreshStart.Models.Notification
{
    public class Notification
    {

        [Key]
        public int NotificationId { get; set; }


        public string UserId { get; set; }


        public string Message { get; set; }


        public bool IsRead { get; set; } = false;


        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}