using WebApplication2.DAL; // מייבא את ה-DAL
using WebApplication2.Models.DTO; // מייבא DTO של משתמשים
using System.Linq; // מייבא FirstOrDefault ותמיכה ב-LINQ

namespace WebApplication2.BLL // מרחב שמות ל-BLL
{ // התחלת namespace
    public class UserServiceBLL : IUserBll // מימוש ממשק ה-BLL
    { // התחלת מחלקה
        private readonly IUserDal _userDal; // תלות ב-DAL

        public UserServiceBLL(IUserDal userDal) // בנאי המקבל את ה-DAL
        { // התחלת בנאי
            _userDal = userDal; // שמירת ה-DAL בשדה
        } // סיום בנאי

        public void AddUser(UserDto userDto) // שיטה לרישום משתמש
        { // התחלת שיטה
            if (string.IsNullOrEmpty(userDto.Role)) // בדיקה אם לא הוגדר תפקיד
            { // התחלת תנאי
                userDto.Role = "Customer"; // ברירת מחדל תפקיד
            } // סיום תנאי

            _userDal.Add(userDto); // שמירה דרך ה-DAL
        } // סיום שיטה

        public UserDto ValidateUser(string email, string password) // אימות משתמש ל-Login
        { // התחלת שיטה
            var allUsers = _userDal.GetAll(); // שליפת כל המשתמשים
            return allUsers.FirstOrDefault(u => u.Email == email && u.Password == password); // החזרת התאמה או null
        } // סיום שיטה
    } // סיום מחלקה
} // סיום namespace