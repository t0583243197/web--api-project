using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class DonorModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string Email { get; set; } = null!;

        [MaxLength(300)]
        public string Address { get; set; } = string.Empty;

        public List<GiftModel>? Gifts { get; set; }

        // שדה להטמעת Soft Delete בעתיד
        public bool IsDeleted { get; set; } = false;
    }
}