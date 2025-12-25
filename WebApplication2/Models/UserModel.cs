using System.Collections.Generic;

namespace WebApplication2.Models
{
    public class UserModel
    {
        public int Id { get; set; } // מפתח ראשי ייחודי למשתמש
        public string Name { get; set; } // שם מלא (נדרש ברישום לקוח)
        public string Email { get; set; } // כתובת מייל לזיהוי ולשליחת הודעות זכייה
        public string Password { get; set; } // סיסמה מוצפנת לצורך התחברות
        public string Role { get; set; } // תפקיד המשתמש (manager/customer) לצורך Authorize
        public List<OrderTicketModel> Tickets { get; set; } // רשימת ההזמנות שביצע המשתמש
    }
}
