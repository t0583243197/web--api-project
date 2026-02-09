using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.DTO
{
    public class CreateDonorDTO
    {
        [Required(ErrorMessage = "שם התורם הוא חובה")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "שם התורם חייב להיות בין 2 ל-100 תווים")]
        public string Name { get; set; } = null!;
        
        [Required(ErrorMessage = "דוא״ל הוא חובה")]
        [EmailAddress(ErrorMessage = "דוא״ל אינו תקני")]
        public string Email { get; set; } = null!;
        
        [StringLength(200, ErrorMessage = "כתובת לא יכולה להיות יותר מ-200 תווים")]
        public string Address { get; set; } = string.Empty;
    }
}
