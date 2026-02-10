using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.DTO
{
    public class CreateOrderRequest
    {
        [Required]
        public int UserId { get; set; }
        
        public decimal TotalAmount { get; set; }
        
        [Required]
        public bool IsDraft { get; set; }
        
        [Required]
        public List<OrderItemRequest> OrderItems { get; set; } = new();
    }

    public class OrderItemRequest
    {
        [Required]
        public int GiftId { get; set; }
        
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
