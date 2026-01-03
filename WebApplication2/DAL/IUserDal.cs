using System.Collections.Generic; // מייבא IEnumerable/List
using System.Threading.Tasks; // מייבא Tasks
using WebApplication2.Models;
using WebApplication2.Models.DTO; // מייבא DTO של משתמש

namespace WebApplication2.DAL // מרחב שמות ל-DAL
{ // התחלת namespace
    public interface IUserDal // ממשק לגישת נתונים של משתמשים
    { // התחלת ממשק
        Task Add(UserDto userDto); // הוספת משתמש חדש
        Task<List<UserDto>> GetAll(); // שליפת כל המשתמשים
        Task Delete(int id); // מחיקת משתמש לפי מזהה
        Task<UserModel> GetFullUserByEmailAsync(string email);
    } // סיום ממשק
} // סיום namespace
