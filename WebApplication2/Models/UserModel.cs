using System.Collections.Generic; // מייבא List
using WebApplication2.Models; // Add this line to import TicketModel if it's in the same namespace

public class UserModel
{
    public int Id { get; set; } // מפתח ראשי ייחודי למשתמש
    public string Name { get; set; } // שם מלא (נדרש ברישום לקוח)
    public string Email { get; set; } // כתובת מייל לזיהוי ולשליחת הודעות זכייה
    public string Password { get; set; } // סיסמה מוצפנת לצורך התחברות
    public string Role { get; set; } // תפקיד המשתמש (manager/customer) לצורך Authorize
    public List<OrderTicketModel> Tickets { get; set; } // רשימת ההזמנות שביצע המשתמש
}
