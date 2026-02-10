using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.DTO
{
    /// <summary>
    /// DTO לפרטי רוכש
    /// </summary>
    public class PurchaserDetailsDto
    {
        /// <summary>שם הרוכש</summary>
        [Required(ErrorMessage = "שם הרוכש הוא חובה")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "שם הרוכש חייב להיות בין 2 ל-100 תווים")]
        public string CustomerName { get; set; }
        
        /// <summary>דוא"ל של הרוכש</summary>
        [Required(ErrorMessage = "דוא״ל הוא חובה")]
        [EmailAddress(ErrorMessage = "דוא״ל אינו תקני")]
        public string Email { get; set; }
        
        /// <summary>מספר כרטיסים שקנה הרוכש</summary>
        [Required(ErrorMessage = "מספר כרטיסים הוא חובה")]
        [Range(1, 10000, ErrorMessage = "מספר הכרטיסים חייב להיות בין 1 ל-10000")]
        public int TicketsCount { get; set; }
    }
}
