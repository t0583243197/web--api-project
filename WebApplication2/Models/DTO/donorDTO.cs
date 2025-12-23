namespace WebApplication2.Models.DTO
{
    public class donorDTO
    {
        public int Id { get; set; } // מזהה התורם לצורך עדכון/מחיקה [cite: 18]
        public string Name { get; set; } // שם התורם לתצוגה ברשימה [cite: 17]

        public string Email { get; set; } // מייל התורם

        public List<GiftDTO>? Gifts { get; set; }
    }
}
