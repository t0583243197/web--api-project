namespace WebApplication2.Models.DTO
{
    public class GiftFilterDto
    {
        public string? GiftName { get; set; } // חיפוש לפי שם המתנה 
        public string? DonorName { get; set; } // חיפוש לפי שם התורם 
        public int? MinPurchasers { get; set; } // סינון לפי מספר רוכשים מינימלי 
        public string? Category { get; set; } // סינון לפי קטגוריה 
        public string? SortBy { get; set; } // מיון לפי מחיר או פופולריות 
    }
}
