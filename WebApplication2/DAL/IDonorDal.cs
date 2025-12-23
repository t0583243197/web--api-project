using WebApplication2.Models.DTO;

namespace WebApplication2.DAL
{
    public interface IDonorDal
    {
        List<donorDTO> GetAll();
        void Add(donorDTO newDonor);
        void Update(donorDTO donorDto); // הוספנו עדכון
        void Delete(int id); // הוספנו מחיקה
    }
}