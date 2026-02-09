using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.DTO
{
    /// <summary>
    /// DTO לתוצאת הגרלה - מתנה וזוכה
    /// </summary>
    public class GiftWinnerDto
    {
        /// <summary>מזהה המתנה</summary>
        [Required(ErrorMessage = "מזהה המתנה הוא חובה")]
        [Range(1, int.MaxValue, ErrorMessage = "מזהה המתנה חייב להיות חיובי")]
        public int GiftId { get; set; }
        
        /// <summary>שם המתנה</summary>
        [Required(ErrorMessage = "שם המתנה הוא חובה")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "שם המתנה חייב להיות בין 2 ל-100 תווים")]
        public string GiftName { get; set; }
        
        /// <summary>שם הזוכה (עשוי להיות ריק אם לא הוחלט זוכה עדיין)</summary>
        [StringLength(100, MinimumLength = 2, ErrorMessage = "שם הזוכה חייב להיות בין 2 ל-100 תווים")]
        public string? WinnerName { get; set; }
    }
}