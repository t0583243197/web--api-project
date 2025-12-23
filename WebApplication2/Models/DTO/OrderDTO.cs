using System.Collections.Generic;

namespace WebApplication2.Models.DTO
{
    public class OrderDTO
    {
        // מזהה המשתמש שמבצע את ההזמנה
        public int UserId { get; set; }

        // רשימת המתנות שהמשתמש בחר לקנות להן כרטיסים
        public List<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();
    }

    public class OrderItemDTO
    {
        public int GiftId { get; set; }
        // כאן אפשר להוסיף כמות אם מאפשרים יותר מכרטיס אחד למתנה באותה הזמנה
        public int Quantity { get; set; } = 1;
    }
}