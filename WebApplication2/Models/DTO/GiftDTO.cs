namespace WebApplication2.Models.DTO // מרחב שמכיל את ה-DTOs
{ // התחלת namespace
    public class GiftDTO // DTO למתנה המשמש ל-API
    { // התחלת מחלקה
        public int Id { get; set; } // חובה עבור עדכון ומחיקה
        public string Name { get; set; } // שם המתנה
        public string? Description { get; set; } // תיאור המתנה
        public decimal TicketPrice { get; set; } // מחיר כרטיס הגרלה
        public string Category { get; set; } // קטגוריה כמחרוזת
        public string DonorName { get; set; } // שם התורם
    } // סיום מחלקה
} // סיום namespace

