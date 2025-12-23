using WebApplication2.Models.DTO;

namespace WebApplication2.BLL
{
    public interface IDonorBLL
    {
        List<donorDTO> GetAllDonors();
        void AddDonor(donorDTO donor);
        void UpdateDonor(donorDTO donor);
        void DeleteDonor(int id);
    }
}