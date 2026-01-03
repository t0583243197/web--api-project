using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class CategoryModel
    {
        public int Id { get; set; } // מזהה קטגוריה

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        public List<GiftModel> Gifts { get; set; } = new List<GiftModel>(); // רשימת המתנות המשויכות לקטגוריה זו 

        // שדה להטמעת Soft Delete בעתיד
        public bool IsDeleted { get; set; } = false;
    }
}
