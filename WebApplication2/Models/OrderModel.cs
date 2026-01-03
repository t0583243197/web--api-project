namespace WebApplication2.Models
{
    public class OrderModel
    {
        // ההזמנה הסופית של כל הכרטיסים שאדם רכש להגרלה
            public int Id { get; set; } // מפתח ראשי להזמנה
            public int UserId { get; set; } // מזהה המשתמש שביצע את הרכישה [cite: 30]
            public UserModel User { get; set; } // אובייקט המשתמש הרוכש
            public bool IsDraft { get; set; } // האם זו טיוטה (בסל) או רכישה סופית [cite: 40, 41]
            public List<OrderTicketModel> OrderItems { get; set; } // רשימת הפריטים שנרכשו בהזמנה זו [cite: 39]
            public DateTime OrderDate { get; set; } // Add this property
            public double TotalAmount { get; set; } // (Optional: add if not present, since used in BLL)
        
    }
}
