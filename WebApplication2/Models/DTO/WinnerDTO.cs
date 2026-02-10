using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.DTO
{
    /// <summary>
    /// DTO לזוכה בהגרלה
    /// </summary>
    public class WinnerDTO
    {
        /// <summary>שם המתנה שנזכה בה</summary>
        [Required(ErrorMessage = "שם המתנה הוא חובה")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "שם המתנה חייב להיות בין 2 ל-100 תווים")]
        public string GiftName { get; set; }
        
        /// <summary>שם הזוכה המאושר</summary>
        [Required(ErrorMessage = "שם הזוכה הוא חובה")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "שם הזוכה חייב להיות בין 2 ל-100 תווים")]
        public string WinnerName { get; set; }
        
        /// <summary>מייל ליצירת קשר ושליחת עדכון</summary>
        [Required(ErrorMessage = "דוא״ל הוא חובה")]
        [EmailAddress(ErrorMessage = "דוא״ל אינו תקני")]
        public string WinnerEmail { get; set; }
    }
}
