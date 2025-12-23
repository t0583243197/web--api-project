namespace WebApplication2.Models // מרחב שמכיל את המודלים
{ // התחלת namespace
    public class GiftModel // מודל מתנה למסד הנתונים
    { // התחלת מחלקה
        public int Id { get; set; } // מזהה מתנה
        public string Name { get; set; } = null!; // שם המתנה
        public string? Description { get; set; } // תיאור אופציונלי
        public decimal TicketPrice { get; set; } // מחיר כרטיס ההגרלה


        public int CategoryId { get; set; } // מפתח זר לקטגוריה
        public CategoryModel Category { get; set; } = null!; // הניווט לקטגוריה

        public int DonorId { get; set; }
        public DonorModel Donnor { get; set; } = null!;

       


    } 
// סיום מחלקה
} // סיום namespace

