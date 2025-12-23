using WebApplication2.Models;

namespace WebApplication2.Models
{
    public class WinnerModel
    {
        public int Id { get; set; } // מזהה זוכה
        public int GiftId { get; set; } // מזהה המתנה שזוכה קיבל
        public GiftModel Gift { get; set; } = null!; // האובייקט המתנה
        public int UserId { get; set; } // מזהה המשתמש הזוכה
        public UserModel User { get; set; } = null!; // האובייקט משתמש
    }
}