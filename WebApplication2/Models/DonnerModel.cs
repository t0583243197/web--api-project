namespace WebApplication2.Models
{
    public class DonorModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string address { get; set; }
        public List<GiftModel>? Gifts { get; set; }
        //[cite: 19]

    }
}
