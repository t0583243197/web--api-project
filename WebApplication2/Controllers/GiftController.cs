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

    [HttpGet] // פתוח לכולם לצפייה בקטלוג
    public IActionResult GetAll() => Ok(_giftBll.getAllGifts()); // החזרת כל המתנות

    [Authorize(Roles = "Manager")] // רק למנהל מחובר
    [HttpPost] // פעולה להוספה
    public IActionResult Add([FromBody] GiftDTO gift) // הוספת מתנה חדשה
    { // התחלת שיטה Add
        _giftBll.addGift(gift); // קריאה ל-BLL להוספת המתנה
        return Ok("המתנה נוספה בהצלחה"); // החזרת הצלחה
    } // סיום שיטה Add

    [Authorize(Roles = "Manager")] // רק למנהל
    [HttpPut] // עדכון מתנה
    public IActionResult Update([FromBody] GiftDTO gift) // עדכון מתנה
    { // התחלת שיטה Update
        _giftBll.updateGift(gift); // קריאה ל-BLL לעדכון
        return Ok("המתנה עודכנה בהצלחה"); // החזרת הצלחה
    } // סיום שיטה Update

    [Authorize(Roles = "Manager")] // רק למנהל
    [HttpDelete("{id}")] // מחיקה לפי Id
    public IActionResult Delete(int id) // מחיקת מתנה
    { // התחלת שיטה Delete
        _giftBll.deleteGift(id); // קריאה ל-BLL למחיקה
        return Ok("המתנה נמחקה מהמערכת"); // החזרת הצלחה
    } // סיום שיטה Delete
} // סיום מחלקה