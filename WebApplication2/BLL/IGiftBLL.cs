using WebApplication2.Models.DTO; // מייבא DTO של מתנה

public interface IGiftBLL // ממשק ללוגיקת מתנות
{ // פתיחת ממשק
    List<GiftDTO> getAllGifts(); // החזרת כל המתנות כ-List של DTO
    List<GiftDTO> GetGiftsByFilter(string? name, string? donorName, int? minPurchasers);// החזרת מתנות לפי סינון
    void addGift(GiftDTO gift); // הוספת מתנה דרך ה-BLL
    void updateGift(GiftDTO gift); // עדכון מתנה דרך ה-BLL
    void deleteGift(int id); // מחיקת מתנה דרך ה-BLL
} // סגירת ממשק