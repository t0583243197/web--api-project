namespace WebApplication2.Models.DTO
{
    public class TicketDTO
    {
        public int Id { get; set; }
        public int GiftId { get; set; }
        public int UserId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool IsUsed { get; set; }
    }
}
