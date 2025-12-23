namespace WebApplication2.Models.DTO
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        // כברירת מחדל, כל מי שנרשם כאן הוא "Customer"
        public string Role { get; set; } = "Customer";
    }
}