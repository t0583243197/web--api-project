namespace WebApplication2.Models
{
    public class OrderTicketModel
    {

        public int Id { get; set; } // מזהה פריט
        public int OrderId { get; set; } // קישור להזמנה האם
        public OrderModel Order { get; set; } // קישור להזמנה 
        public int GiftId { get; set; } // קישור למתנה שנרכשה
        public GiftModel Gift { get; set; } // אובייקט המתנה
        public int Quantity { get; set; } // כמות הכרטיסים שנרכשו למתנה זו [cite: 39]


        
           
        


    }
}
