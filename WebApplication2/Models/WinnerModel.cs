using WebApplication2.Models;

namespace WebApplication2.Models
{
    public class WinnerModel
    {
        public int Id { get; set; }
        public int GiftId { get; set; }
        public GiftModel Gift { get; set; }
        public int UserId { get; set; }
        public UserModel User { get; set; }
    }
}