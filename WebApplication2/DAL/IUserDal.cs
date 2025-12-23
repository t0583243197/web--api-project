using WebApplication2.Models.DTO; // מייבא DTO של משתמש
using System.Collections.Generic; // מייבא IEnumerable/List

namespace WebApplication2.DAL // מרחב שמות ל-DAL
{ // התחלת namespace
    public interface IUserDal // ממשק לגישת נתונים של משתמשים
    { // התחלת ממשק
        void Add(UserDto userDto); // הוספת משתמש חדש
        IEnumerable<UserDto> GetAll(); // שליפת כל המשתמשים
    } // סיום ממשק
} // סיום namespace