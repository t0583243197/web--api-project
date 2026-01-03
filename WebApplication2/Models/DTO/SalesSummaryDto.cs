namespace WebApplication2.Models.DTO
{
    public class SalesSummaryDto
    { //SalesSummaryDto: מספק סיכום של מכירות כולל סך ההכנסות
      //, מספר הכרטיסים שנמכרו
      //, ופירוט המכירות לפי מתנה.
        public decimal TotalRevenue { get; set; } // סך כל ההכנסות מהמכירה [cite: 34]
        public int TotalTicketsSold { get; set; } // סך הכרטיסים שנמכרו
        public List<GiftSalesDto> SalesPerGift { get; set; } // פירוט מכירות לכל מתנה [cite: 28]
    }

    public class GiftSalesDto 
    {//GiftSalesDto: מייצגת את הנתונים עבור מכירה של מתנה מסוימת
     //, כולל שם המתנה
     //, מספר הרוכשים שלה,
     //וההכנסה שנוצרה ממנה.
        public string GiftName { get; set; } // שם המתנה [cite: 28]
        public int PurchaseCount { get; set; } // מספר הרוכשים למתנה זו 
        public decimal RevenueFromGift { get; set; } // הכנסה ספציפית מהמתנה
    }
}
