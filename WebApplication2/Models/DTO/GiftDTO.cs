using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApplication2.Models.DTO
{
    /// <summary>
    /// DTO למתנה המשמש ל-API
    /// </summary>
    public class GiftDTO
    {
        /// <summary>מזהה ייחודי של המתנה</summary>
        [Range(1, int.MaxValue, ErrorMessage = "מזהה המתנה חייב להיות חיובי")]
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        /// <summary>שם המתנה</summary>
        [Required(ErrorMessage = "שם המתנה הוא חובה")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "שם המתנה חייב להיות בין 2 ל-100 תווים")]
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        /// <summary>תיאור המתנה</summary>
        [StringLength(500, ErrorMessage = "התיאור לא יכול להיות יותר מ-500 תווים")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        
        /// <summary>כתובת תמונה של המתנה</summary>
        [StringLength(2000, ErrorMessage = "כתובת התמונה לא יכולה להיות יותר מ-2000 תווים")]
        [JsonPropertyName("imageUrl")]
        public string? ImageUrl { get; set; }
        
        /// <summary>מחיר כרטיס הגרלה</summary>
        [Required(ErrorMessage = "מחיר הכרטיס הוא חובה")]
        [Range(0.01, 10000, ErrorMessage = "מחיר הכרטיס חייב להיות בין 0.01 ל-10000")]
        [JsonPropertyName("ticketPrice")]
        public decimal TicketPrice { get; set; }
        
        /// <summary>קטגוריה של המתנה</summary>
        [Required(ErrorMessage = "קטגוריה היא חובה")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "קטגוריה חייבת להיות בין 1 ל-50 תווים")]
        [JsonPropertyName("category")]
        public string Category { get; set; }
        
        /// <summary>שם התורם של המתנה</summary>
        [Required(ErrorMessage = "שם התורם הוא חובה")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "שם התורם חייב להיות בין 2 ל-100 תווים")]
        [JsonPropertyName("donorName")]
        public string DonorName { get; set; }
        
        [JsonPropertyName("ticketsSold")]
        public int? TicketsSold { get; set; }
        
        [JsonPropertyName("hasWinner")]
        public bool? HasWinner { get; set; }
    }
}

