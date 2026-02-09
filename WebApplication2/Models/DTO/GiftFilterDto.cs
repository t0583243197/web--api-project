using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.DTO
{
    /// <summary>
    /// DTO לסינון חיפוש של מתנות
    /// </summary>
    public class GiftFilterDto
    {
        /// <summary>חיפוש לפי שם המתנה</summary>
        [StringLength(100, ErrorMessage = "שם המתנה לא יכול להיות יותר מ-100 תווים")]
        public string? GiftName { get; set; }
        
        /// <summary>חיפוש לפי שם התורם</summary>
        [StringLength(100, ErrorMessage = "שם התורם לא יכול להיות יותר מ-100 תווים")]
        public string? DonorName { get; set; }
        
        /// <summary>סינון לפי מספר רוכשים מינימלי</summary>
        [Range(0, 10000, ErrorMessage = "מספר הרוכשים חייב להיות בין 0 ל-10000")]
        public int? MinPurchasers { get; set; }
        
        /// <summary>סינון לפי קטגוריה</summary>
        [StringLength(50, ErrorMessage = "קטגוריה לא יכולה להיות יותר מ-50 תווים")]
        public string? Category { get; set; }
        
        /// <summary>מיון לפי מחיר או פופולריות</summary>
        [StringLength(20, ErrorMessage = "סוג המיון לא יכול להיות יותר מ-20 תווים")]
        [RegularExpression(@"^(price|popularity)$", ErrorMessage = "סוג המיון חייב להיות 'price' או 'popularity'")]
        public string? SortBy { get; set; }
    }
}
