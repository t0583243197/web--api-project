namespace WebApplication2.Models.DTO
{
    public class DonorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = string.Empty;

        // Ensure this is a DTO list, not model list
        public List<GiftDTO> Gifts { get; set; } = new List<GiftDTO>();
    }
}
