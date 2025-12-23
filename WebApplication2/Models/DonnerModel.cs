namespace WebApplication2.Models
{
    public class DonorModel
    {
        public int Id { get; set; } // מפתח ראשי לתורם
        public string Name { get; set; } // שם התורם [cite: 20]
        public string Email { get; set; } // מייל התורם [cite: 20]
        public List<GiftModel> Gifts { get; set; } // רשימת המתנות שהתורם תרם [cite: 19]
    }
}
