using WebApplication2.Models.DTO; // מייבא DTO של מתנה

namespace WebApplication2.DAL // מרחב שמות ל-DAL
{ // פתיחת namespace
    public interface IGiftDal // ממשק לגישת נתונים של מתנות
    { // פתיחת ממשק
        List<GiftDTO> getAll(); // החזרת כל המתנות כ-List של DTO
        void add(GiftDTO gift); // הוספת מתנה חדשה
        void update(GiftDTO gift); // עדכון מתנה קיימת
        void delete(int id); // מחיקת מתנה לפי Id
    } // סגירת ממשק
} // סגירת namespace