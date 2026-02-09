using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.DTO
{
    public class CreateGiftDTO
    {
        [Required(ErrorMessage = "שם המתנה הוא חובה")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "שם המתנה חייב להיות בין 2 ל-200 תווים")]
        public string Name { get; set; } = null!;

        [StringLength(500, ErrorMessage = "תיאור לא יכול להיות יותר מ-500 תווים")]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, 10000, ErrorMessage = "מחיר הכרטיס חייב להיות בין 0.01 ל-10000")]
        public decimal TicketPrice { get; set; }

        public string? Category { get; set; }
        
        public string? DonorName { get; set; }
    }
}
