using System.Collections.Generic;

namespace WebApplication2.Models
{
    public class DonorModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = string.Empty;
        public List<GiftModel>? Gifts { get; set; }
    }
}