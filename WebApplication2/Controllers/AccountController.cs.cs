using Microsoft.AspNetCore.Mvc; // מייבא ControllerBase ו-Attributes
using Microsoft.IdentityModel.Tokens; // מייבא סוגי חתימה ל-JWT
using System.IdentityModel.Tokens.Jwt; // מייבא טיפול ב-JWT
using System.Security.Claims; // מייבא יצירת Claims
using System.Text; // מייבא המרת מחרוזת לבייטים
using WebApplication2.Models.DTO; // מייבא DTOs של משתמשים
using WebApplication2.BLL; // מייבא את שירות ה-BLL של משתמשים


namespace WebApplication2.Controllers // מרחב שמות לבקרים
{ // התחלת namespace
    [ApiController] // מציין API Controller
    [Route("api/[controller]")] // נתיב ניתוב
    public class AccountController : ControllerBase // בקר לטיפול בהרשאות
    { // התחלת מחלקה
        private readonly IUserBll _userBll; // תלות ב-BLL

        public AccountController(IUserBll userBll) // בנאי שמקבל את ה-BLL
        { // התחלת בנאי
            _userBll = userBll; // שמירת ה-BLL בשדה
        } // סיום בנאי

        [HttpPost("login")] // POST api/account/login
        public IActionResult Login([FromBody] LoginDTO login) // כניסה המתקבלת כ-LoginDTO
        { // התחלת שיטה
            var user = _userBll.ValidateUser(login.Email, login.Password); // אימות מול BLL

            if (user != null) // אם найден משתמש תואם
            { // התחלת תנאי
                var tokenHandler = new JwtSecurityTokenHandler(); // יוצר JWT handler
                var key = Encoding.ASCII.GetBytes("YourSuperSecretKeyHere1234567890!"); // מפתח סימטרי לדוגמה

                var tokenDescriptor = new SecurityTokenDescriptor // הגדרת פרטי הטוקן
                { // התחלת אובייקט
                    Subject = new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Email, user.Email), // Claim של אימייל
                        new Claim(ClaimTypes.Role, user.Role) // Claim של תפקיד
                    }), // סיום Claims
                    Expires = DateTime.UtcNow.AddHours(2), // תוקף הטוקן
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) // קרדנציאל חתימה
                }; // סיום tokenDescriptor

                var token = tokenHandler.CreateToken(tokenDescriptor); // יצירת הטוקן
                return Ok(new { token = tokenHandler.WriteToken(token) }); // החזרת טוקן ללקוח
            } // סיום תנאי

            return Unauthorized("שם משתמש או סיסמה שגויים"); // החזרת שגיאה אם לא תואם
        } // סיום שיטה Login

        [HttpPost("register")] // POST api/account/register
        public IActionResult Register([FromBody] UserDto userDto) // רישום משתמש חדש
        { // התחלת שיטה
            _userBll.AddUser(userDto); // שליחה ל-BLL לשמירה
            return Ok(new { message = "המשתמש נרשם בהצלחה! כעת ניתן לבצע Login" }); // תשובת הצלחה
        } // סיום שיטה Register
    } // סיום מחלקה
} // סיום namespace