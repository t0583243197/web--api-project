using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.DTO
{
    /// <summary>
    /// DTO להזמנה כוללת של כרטיסי המתנות
    /// </summary>
    public class OrderDTO
    {
        public int Id { get; set; }
        
        /// <summary>מזהה המשתמש שמבצע את ההזמנה</summary>
        public int UserId { get; set; }
        
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsDraft { get; set; }

        /// <summary>סכום כולל של ההזמנה</summary>
        [Required(ErrorMessage = "סכום כולל הוא חובה")]
        [Range(0.01, double.MaxValue, ErrorMessage = "סכום כולל חייב להיות גדול מ-0")]
        public double TotalAmount { get; set; }
        
        /// <summary>רשימת פרטי ההזמנה</summary>
        [Required(ErrorMessage = "רשימת פריטים היא חובה")]
        [MinLength(1, ErrorMessage = "חייב להיות לפחות פרט אחד בהזמנה")]
        public List<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
    }

    /// <summary>
    /// DTO לפרית בודד בהזמנה
    /// </summary>
    public class OrderItemDTO
    {
        /// <summary>מזהה המתנה</summary>
        [Required(ErrorMessage = "מזהה המתנה הוא חובה")]
        [Range(1, int.MaxValue, ErrorMessage = "מזהה המתנה חייב להיות חיובי")]
        public int GiftId { get; set; }
        
        public string? GiftName { get; set; }
        
        /// <summary>כמות הכרטיסים למתנה זו</summary>
        [Required(ErrorMessage = "כמות היא חובה")]
        [Range(1, 100, ErrorMessage = "כמות חייבת להיות בין 1 ל-100")]
        public int Quantity { get; set; } = 1;
    }
}