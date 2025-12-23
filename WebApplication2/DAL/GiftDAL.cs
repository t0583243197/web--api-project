using AutoMapper; // מייבא AutoMapper
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL; // מייבא מרחב ה-DAL
using WebApplication2.Models; // מייבא מודלים
using WebApplication2.Models.DTO; // מייבא DTOs

public class GiftDAL : IGiftDal // מימוש DAL עבור מתנות
{ // התחלת מחלקה
    private readonly StoreContext _context; // שדה לדטה-קונטקסט
    private readonly IMapper _mapper; // שדה ל-Mapper

    public GiftDAL(StoreContext context, IMapper mapper) // בנאי המכניס תלותיות
    { // התחלת בנאי
        _context = context; // שמירת הקונטקסט
        _mapper = mapper; // שמירת ה-Mapper
    } // סיום בנאי

    public List<GiftDTO> GetByFilter(string? name, string? donorName, int? minPurchasers)
    {
        // 1. התחלת שאילתה - נשאר ב-IQueryable (עדיין לא רץ ב-SQL)
        var query = _context.Gifts
            .Include(g => g.Donnor)
            .AsQueryable();

        // 2. הוספת תנאי חיפוש לפי שם מתנה
        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(g => g.Name.Contains(name));
        }

        // 3. הוספת תנאי חיפוש לפי שם תורם
        if (!string.IsNullOrEmpty(donorName))
        {
            query = query.Where(g => g.Donnor.Name.Contains(donorName));
        }

        // 4. חשוב: ביצוע ה-ToList רק כאן! 
        // השאילתה שתשלח ל-SQL תהיה קטנה ומדויקת יותר.
        var gifts = query.ToList();

        // 5. מיפוי ל-DTO
        return _mapper.Map<List<GiftDTO>>(gifts);
    }
    public void add(GiftDTO giftDto) // הוספת מתנה חדשה
    { // התחלת שיטה
        var gift = _mapper.Map<GiftModel>(giftDto); // המרת DTO למודל
        _context.Gifts.Add(gift); // הוספת המודל ל-DbSet
        _context.SaveChanges(); // שמירה ל-DB
    } // סיום שיטה

    public void update(GiftDTO giftDto) // עדכון מתנה קיימת
    { // התחלת שיטה
        var existingGift = _context.Gifts.Find(giftDto.Id); // מציאת מתנה לפי Id
        if (existingGift != null) // אם נמצא
        { // התחלת תנאי
            _mapper.Map(giftDto, existingGift); // עדכון השדות בתורם הקיים
            _context.SaveChanges(); // שמירה ל-DB
        } // סיום תנאי
    } // סיום שיטה

    public void delete(int id) // מחיקת מתנה
    { // התחלת שיטה
        var gift = _context.Gifts.Find(id); // מציאת מתנה לפי Id
        if (gift != null) // אם נמצא
        { // התחלת תנאי
            _context.Gifts.Remove(gift); // הסרת המתנה
            _context.SaveChanges(); // שמירה ל-DB
        } // סיום תנאי
    } // סיום שיטה

    public List<GiftDTO> getAll()
    {
        var gifts = _context.Gifts
            .Include(g => g.Donnor)
            .ToList();
        return _mapper.Map<List<GiftDTO>>(gifts);
    }
} // סיום מחלקה