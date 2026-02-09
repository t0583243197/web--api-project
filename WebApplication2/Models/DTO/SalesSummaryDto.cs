using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.DTO
{
    /// <summary>
    /// DTO לסיכום מכירות כללי
    /// </summary>
    public class SalesSummaryDto
    {
        /// <summary>סך כל ההכנסות מהמכירה</summary>
        [Required(ErrorMessage = "סך ההכנסות הוא חובה")]
        [Range(0.0, 10000000.0, ErrorMessage = "סך ההכנסות לא יכול להיות שלילי")]
        public decimal TotalRevenue { get; set; }
        
        /// <summary>מספר ההזמנות המאושרות</summary>
        [Range(0, int.MaxValue, ErrorMessage = "מספר ההזמנות לא יכול להיות שלילי")]
        public int TotalOrders { get; set; }
        
        /// <summary>סך הכרטיסים שנמכרו</summary>
        [Required(ErrorMessage = "מספר הכרטיסים הוא חובה")]
        [Range(0, int.MaxValue, ErrorMessage = "מספר הכרטיסים לא יכול להיות שלילי")]
        public int TotalTicketsSold { get; set; }
        
        /// <summary>פירוט מכירות לכל מתנה</summary>
        public List<GiftSalesDto> SalesPerGift { get; set; } = new List<GiftSalesDto>();
    }

    /// <summary>
    /// DTO לפרטי מכירה של מתנה מסוימת
    /// </summary>
    public class GiftSalesDto
    {
        /// <summary>שם המתנה</summary>
        [Required(ErrorMessage = "שם המתנה הוא חובה")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "שם המתנה חייב להיות בין 2 ל-100 תווים")]
        public string GiftName { get; set; }
        
        /// <summary>מספר הרוכשים/כרטיסים למתנה זו</summary>
        [Required(ErrorMessage = "מספר הרוכשים הוא חובה")]
        [Range(0, 100000, ErrorMessage = "מספר הרוכשים לא יכול להיות שלילי")]
        public int PurchaseCount { get; set; }
        
        /// <summary>ההכנסה ספציפית מהמתנה</summary>
        [Required(ErrorMessage = "ההכנסה היא חובה")]
        [Range(0.0, 10000000.0, ErrorMessage = "ההכנסה לא יכולה להיות שלילית")]
        public decimal RevenueFromGift { get; set; }
    }
}
