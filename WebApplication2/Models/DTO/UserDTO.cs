using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.DTO
{
    public class UserDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [Phone]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "הטלפון חייב להכיל ספרות בלבד")]
        public string Phone { get; set; }
        
        public string Password { get; set; }
        // כברירת מחדל, כל מי שנרשם כאן הוא "Customer"
        public string Role { get; set; } = "Customer";
    }
}