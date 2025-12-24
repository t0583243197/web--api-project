using Microsoft.AspNetCore.Authorization; // מייבא Authorize attribute
using Microsoft.AspNetCore.Mvc; // מייבא MVC attributes
using WebApplication2.Models.DTO; // מייבא DTOs של מתנות

[ApiController] // מציין Controller מסוג API
[Route("api/[controller]")] // נתיב ניתוב
public class GiftsController : ControllerBase // בקר לטיפול במתנות
{ // התחלת מחלקה
    private readonly IGiftBLL _giftBll; // שדה ל-BLL של מתנות

    public GiftsController(IGiftBLL giftBll) // בנאי המקבל את ה-BLL
    { // התחלת בנאי
        _giftBll = giftBll; // שמירת ה-BLL בשדה
    } // סיום בנאי

    [HttpGet]
    public async Task<ActionResult<List<GiftDTO>>> Get([FromQuery] string? name, [FromQuery] string? donorName, [FromQuery] int? minPurchasers)
    {
        // אם לא נשלחו פרמטרים, זה יחזיר את כל המתנות.
        // אם נשלחו, ה-BLL יבצע את הסינון שכתבנו ב-DAL.
        var gifts = await _giftBll.GetGiftsByFilterAsync(name, donorName, minPurchasers);
        return Ok(gifts);
    }

    [Authorize(Roles = "Manager")] // רק למנהל מחובר
    [HttpPost] // פעולה להוספה
    public async Task<IActionResult> Add([FromBody] GiftDTO gift) // הוספת מתנה חדשה
    { // התחלת שיטה Add
        await _giftBll.AddGiftAsync(gift); // קריאה ל-BLL להוספת המתנה
        return Ok("המתנה נוספה בהצלחה"); // החזרת הצלחה
    } // סיום שיטה Add

    [Authorize(Roles = "Manager")] // רק למנהל
    [HttpPut] // עדכון מתנה
    public async Task<IActionResult> Update([FromBody] GiftDTO gift) // עדכון מתנה
    { // התחלת שיטה Update
        await _giftBll.UpdateGiftAsync(gift); // קריאה ל-BLL לעדכון
        return Ok("המתנה עודכנה בהצלחה"); // החזרת הצלחה
    } // סיום שיטה Update

    [Authorize(Roles = "Manager")] // רק למנהל
    [HttpDelete("{id}")] // מחיקה לפי Id
    public async Task<IActionResult> Delete(int id) // מחיקת מתנה
    { // התחלת שיטה Delete
        await _giftBll.DeleteGiftAsync(id); // קריאה ל-BLL למחיקה
        return Ok("המתנה נמחקה מהמערכת"); // החזרת הצלחה
    } // סיום שיטה Delete
      // 1. נתיב למיון לפי המחיר הגבוה ביותר
    [HttpGet("sorted-by-price")]
    public async Task<IActionResult> GetGiftsByPrice()
    {
        var gifts = await _giftBll.GetGiftsSortedByPriceAsync();
        return Ok(gifts);
    }

    // 2. נתיב למיון לפי המתנה הנרכשת ביותר
    [HttpGet("most-purchased")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> GetMostPurchased()
    {
        var gifts = await _giftBll.GetMostPurchasedGiftsAsync();
        return Ok(gifts);
    }

    // 3. עדכון ה-Get הקיים כדי לתמוך בסינון המלא (כולל minPurchasers)
  
} // סיום מחלקה