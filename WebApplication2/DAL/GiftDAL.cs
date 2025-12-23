using AutoMapper; // מייבא AutoMapper
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

    public List<GiftDTO> getAll() => _mapper.Map<List<GiftDTO>>(_context.Gifts.ToList()); // החזרת כל המתנות כ-DTO

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
} // סיום מחלקה