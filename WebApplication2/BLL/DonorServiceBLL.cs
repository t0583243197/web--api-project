using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.DAL;
using WebApplication2.Models.DTO;

namespace WebApplication2.BLL
{
    public class DonorServiceBLL : IDonorBLL
    {
        private readonly IDonorDal _donorDal;

        public DonorServiceBLL(IDonorDal donorDal) => _donorDal = donorDal;

        public Task<List<DonorDTO>> GetAllDonorsAsync() => _donorDal.GetAllAsync();

        public Task<List<DonorDTO>> GetDonorsByFilterAsync(string? name, string? email, string? giftName)
            => _donorDal.GetByFilterAsync(name, email, giftName);

        public Task AddDonorAsync(DonorDTO donor) => _donorDal.AddAsync(donor);

        public Task UpdateDonorAsync(DonorDTO donor) => _donorDal.UpdateAsync(donor);

        public Task DeleteDonorAsync(int id) => _donorDal.DeleteAsync(id);
    }
}