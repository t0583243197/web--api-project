namespace WebApplication2.Models.DTO
{
    public class DonorDTO
    {
        public int Id { get; set; } // מזהה התורם לצורך עדכון/מחיקה
        public string Name { get; set; } // שם התורם לתצוגה ברשימה

        public string Email { get; set; } // מייל התורם
        public string? Address { get; set; } // כתובת התורם

        public List<GiftDTO>? Gifts { get; set; }
    }
}
