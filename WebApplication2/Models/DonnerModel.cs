namespace WebApplication2.Models
{
    public class DonorModel
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public List<GiftModel>? Gifts { get; set; }
        //[cite: 19]

    }
}
