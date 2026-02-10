using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.DTO
{
    /// <summary>
    /// DTO לכניסת משתמש
    /// </summary>
    public class LoginDTO
    {
        /// <summary>דוא"ל של המשתמש</summary>
        [Required(ErrorMessage = "דוא״ל הוא חובה")]
        [EmailAddress(ErrorMessage = "דוא״ל אינו תקני")]
        public string Email { get; set; } = null!;
        
        /// <summary>סיסמה של המשתמש</summary>
        [Required(ErrorMessage = "סיסמה היא חובה")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "סיסמה חייבת להיות לפחות 6 תווים")]
        public string Password { get; set; } = null!;
    }
}