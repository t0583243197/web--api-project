using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models // מרחב שמכיל את המודלים
{ // התחלת namespace
    public class GiftModel // מודל מתנה למסד הנתונים
    { // התחלת מחלקה
        public int Id { get; set; } // מזהה מתנה

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!; // שם המתנה

        [MaxLength(1000)]
        public string? Description { get; set; } // תיאור אופציונלי

        [Required]
        public decimal TicketPrice { get; set; } // מחיר כרטיס ההגרלה

        [Required]
        public int CategoryId { get; set; } // מפתח זר לקטגוריה

        [Required]
        public CategoryModel Category { get; set; } = null!; // הניווט לקטגוריה

        [Required]
        public int DonorId { get; set; } // מזהה תורם

        [Required]
        public DonorModel Donor { get; set; } = null!; // הניווט לתורם

        // שדה להטמעת Soft Delete בעתיד
        public bool IsDeleted { get; set; } = false;
    } 
// סיום מחלקה
} // סיום namespace

