using WebApplication2.Models.DTO; // כולל DTOs של המשתמש

namespace WebApplication2.BLL // מרחב ה-Business Logic Layer
{ // התחלת namespace
    public interface IUserBll // ממשק עבור לوجיקת המשתמשים ברמה העסקית
    { // התחלת ממשק
        void AddUser(UserDto userDto); // הגדרה לשיטת רישום/הוספת משתמש

        // Add this method to support user validation
        UserDto ValidateUser(string email, string password); // הגדרה לשיטת אימות משתמש
    } // סיום ממשק IUserBll
} // סיום namespace WebApplication2.BLL