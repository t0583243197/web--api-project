using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models.DTO;

namespace WebApplication2.DAL
{
    public interface IDonorDal
    {
        Task<List<DonorDTO>> GetAllAsync();
        Task<List<DonorDTO>> GetByFilterAsync(string? name, string? email, string? giftName);
        Task AddAsync(DonorDTO newDonor);
        Task UpdateAsync(DonorDTO donorDto);
        Task DeleteAsync(int id);
    }
}