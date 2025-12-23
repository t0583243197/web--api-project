using Microsoft.EntityFrameworkCore; // מייבא EF Core
using WebApplication2.Models.DTO; // מייבא DTO של משתמש
using AutoMapper; // מייבא AutoMapper למיפוי בין DTO ל-Model
using System.Collections.Generic; // מייבא IEnumerable/List

namespace WebApplication2.DAL // מרחב שמות ל-DAL
{ // התחלת namespace
    public class UserDAL : IUserDal // מימוש ממשק ה-DAL של משתמשים
    { // התחלת מחלקה
        private readonly IMapper _mapper; // שדה ל-Mapper
        private readonly StoreContext _context; // שדה ל-DbContext

        public UserDAL(StoreContext context, IMapper mapper) // בנאי המקבל תלותיות
        { // התחלת בנאי
            _context = context; // שמירת הקונטקסט
            _mapper = mapper; // שמירת ה-Mapper
        } // סיום בנאי

        public void Add(UserDto userDto) // הוספת משתמש חדש
        { // התחלת שיטה Add
            var userModel = _mapper.Map<UserModel>(userDto); // המרת DTO ל-Model
            _context.Users.Add(userModel); // הוספה ל-DbSet
            _context.SaveChanges(); // שמירה למסד הנתונים
        } // סיום שיטה Add

        public IEnumerable<UserDto> GetAll() // שליפת כל המשתמשים כ-DTO
        { // התחלת שיטה GetAll
            var users = _context.Users.ToList(); // שליפת כל ה-UserModel
            return _mapper.Map<IEnumerable<UserDto>>(users); // המרת הרשימה ל-DTO והחזרתה
        } // סיום שיטה GetAll
    } // סיום מחלקה
} // סיום namespace
