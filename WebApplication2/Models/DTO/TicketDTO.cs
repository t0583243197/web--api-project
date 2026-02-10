using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.DTO
{
    /// <summary>
    /// DTO לכרטיס הגרלה
    /// </summary>
    public class TicketDTO
    {
        /// <summary>מזהה ייחודי של הכרטיס</summary>
        [Range(1, int.MaxValue, ErrorMessage = "מזהה הכרטיס חייב להיות חיובי")]
        public int Id { get; set; }
        
        /// <summary>מזהה המתנה הקשורה לכרטיס</summary>
        [Required(ErrorMessage = "מזהה המתנה הוא חובה")]
        [Range(1, int.MaxValue, ErrorMessage = "מזהה המתנה חייב להיות חיובי")]
        public int GiftId { get; set; }
        
        /// <summary>מזהה המשתמש שקנה את הכרטיס</summary>
        [Required(ErrorMessage = "מזהה המשתמש הוא חובה")]
        [Range(1, int.MaxValue, ErrorMessage = "מזהה המשתמש חייב להיות חיובי")]
        public int UserId { get; set; }
        
        /// <summary>תאריך קנייה של הכרטיס</summary>
        public DateTime PurchaseDate { get; set; }
        
        /// <summary>האם הכרטיס כבר שומש (זוכה או לא)</summary>
        public bool IsUsed { get; set; }
    }
}
