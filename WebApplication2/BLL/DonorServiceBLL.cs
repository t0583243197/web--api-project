using WebApplication2.DAL;
using WebApplication2.Models.DTO;

namespace WebApplication2.BLL
{
    public class DonorServiceBLL : IDonorBLL
    {
        private readonly IDonorDal _donorDal;

        public DonorServiceBLL(IDonorDal donorDal) => _donorDal = donorDal;

        public List<donorDTO> GetAllDonors() => _donorDal.GetAll();
        public void AddDonor(donorDTO donor) => _donorDal.Add(donor);
        public void UpdateDonor(donorDTO donor) => _donorDal.Update(donor);
        public void DeleteDonor(int id) => _donorDal.Delete(id);
    }
}