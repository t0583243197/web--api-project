namespace WebApplication2.Models
{
    public class TicketModel
    {
    
        
            public int Id { get; set; }
            public int GiftId { get; set; }
            public int UserId { get; set; }
            public DateTime PurchaseDate { get; set; }
            public bool IsUsed { get; set; }
        
    }
}