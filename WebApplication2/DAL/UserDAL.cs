using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models.DTO;
using AutoMapper;
using AutoMapper.QueryableExtensions; // <-- Add this using directive
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;
using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace WebApplication2.DAL
{
    public class UserDAL : IUserDal
    {
        private readonly IMapper _mapper;
        private readonly StoreContext _context;

        public UserDAL(StoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        //המרה בין UserRole למחרוזת
        private static readonly ValueConverter<UserRole, string> _userRoleConverter = new ValueConverter<UserRole, string>(
            v => v.ToString(),
            v => Enum.Parse<UserRole>(v, true));

        public async Task Add(UserDto userDto)
        {
            Console.WriteLine($"=== UserDAL.Add called for: {userDto.Email} ===");

            if (!Enum.TryParse<UserRole>(userDto.Role, true, out _))
                throw new ArgumentException("Invalid role");

            var userModel = _mapper.Map<UserModel>(userDto);
            Console.WriteLine($"Mapped to UserModel: {userModel.Email}, Role: {userModel.Role}");
            
            _context.Users.Add(userModel);
            Console.WriteLine("Added to context");
            
            var result = await _context.SaveChangesAsync();
            Console.WriteLine($"SaveChanges result: {result} rows affected");
            
            // בדיקה נוספת
            var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
            Console.WriteLine($"User found after save: {savedUser != null}");
        }

        // Read-only: use ProjectTo and AsNoTracking.
        // Ensure AutoMapper mapping UserModel -> UserDto excludes Password so EF doesn't fetch it.
        public async Task<List<UserDto>> GetAll()
        { 
                return await _context.Users
                .AsNoTracking()                // שלא יעקוב אחרי השינויים כיון שזה שליפה בלבד וכך חוסך בזיכרון
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)//המרת הרשימה שחוזרת מה
                                                                  //DB
                                                                  //לרשימה של
                                                                  //DTO
                .ToListAsync();                                // ביצוע השאילתה וחזרת התוצאה כרשימה
        }

        // Soft-delete for User
        public async Task Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }

        // Connected partial update: update only the specified properties.
        // Example usage:
        // await UpdatePartialAsync(userId, u => u.Role = UserRole.Manager, u => u.Role);
        //הפונקציה הזו מאפשרת לך לעדכן אובייקט מסוים בצורה חלקית. כלומר,
        //במקום לבצע עדכון לכל שדות האובייקט
        //, אתה מספק רשימה של שדות או ערכים ששונו, ומשתמש בפונקציה
        //כדי לקבוע את הערכים החדשים.
        //Entity Framework  ידע לעדכן רק את השדות שהוזכרו.
        //  נשתמש בפונקציה הזו כאשר לא כל השדות צריכים
        //  להשתנות באוביקטים גדולים בשביל יעילות
        public async Task UpdatePartialAsync(int id, Action<UserModel> setValues, params Expression<Func<UserModel, object>>[] modifiedProperties)
        {// הסברת מושג delegation
         // זה העברת אחריות לפונקציה אחרת
         //   גמישות(Flexibility) נשתמש בו בשביל 
         //  כל פעם נוכל להגיד לו לשנות שדות שונים
         //בלי לשנות את הפונקציה המרכזית
         //  וגם הפרדת אחריות הפונקציה המנהלת לא צריכה לדעת איך לעדכן  
         //מאפשר לכתוב פונקציה אחת לכול סוגי העדכונים 
         //Action<UserModel> הוא
         //delegation של
         //פונקציה שמקבלת אובייקט מסוג
         //UserModel
         //ומשנה אותו, אך לא מחזירה שום ערך.
         //הפונקציה setValues
         //מאפשרת להחיל שינויים על אובייקט
         //UserModel.
         //params Expression<Func<UserModel, object>>[]
         //המערך הזה מכיל את השדות של
         //UserModel
         //שהשתנו, כך שנוכל לדעת אילו שדות יש לעדכן במסד הנתונים.
            var entity = new UserModel { Id = id };
            _context.Users.Attach(entity);// מצמיד את האובייקט להקשר של
                                          // EF
                                          //זה מאפשר ל-
                                         //Entity Framework
                                          //לדעת שהאובייקט הזה עשוי להשתנות,
                                          //ואם יהיו שינויים,
                                          //הוא ידע לעדכן אותם במסד הנתונים
            setValues(entity);//שם את הערכים החדשים לאובייקט
                                
            var entry = _context.Entry(entity);
            //השורה הזו מקבלת את ה-"entry" של האובייקט בקשר.
            //ה-entry
            //הוא אובייקט המייצג את המצב הנוכחי של האובייקט בתוך הקשר ה
            //-DbContext.
            //ניתן להשתמש בו כדי לגשת למידע כמו אם השדה שונה או אם צריך לעדכן אותו במסד.
            foreach (var prop in modifiedProperties)
            {
                var propName = GetPropertyName(prop);
                //זיהוי השדות (The Identification):
                //כאן נכנסת הלולאה שכתבת. ה - GetPropertyName
                //אומר למערכת: "הנה השמות של העמודות שצריך לעדכן"
                //.זהו שלב המיפוי
                //.
                entry.Property(propName).IsModified = true;
                //IsModified = true, כלומר, ה-Entity Framework
                //יידע שיש לשדה הזה ערך חדש ויש לעדכן אותו במסד הנתונים.
            }

            await _context.SaveChangesAsync();
        }

        // Helper to extract property name from expression
        //הפונקציה GetPropertyName<T> מקבלת ביטוי
        //(expression) שמפנה לשדה או למאפיין (property) של אובייקט,
        //ומחזירה את שם השדה או המאפיין הזה כ-
        //string
        private static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            if (expression.Body is MemberExpression member)//MemberExpression –
                                                           //זהו הביטוי שמצביע ישירות על שדה או מאפיין
                return member.Member.Name;

            if (expression.Body is UnaryExpression unary && unary.Operand is MemberExpression memberOperand)
                return memberOperand.Member.Name;
            //unaryExpression – אם יש המרת טיפוס (כמו ב-(object)x.Name)
            //אלה שתי דרכים  על מנת לחלץ את שם המאפין מתוך הביטוי.
            throw new ArgumentException("Invalid expression");
        }
     
        public async Task<UserModel> GetFullUserByEmailAsync(string email)
        { //שליפת משתמש מלא לפי אימייל
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        }
    }
}
