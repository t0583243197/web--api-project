using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models.DTO;

namespace WebApplication2.DAL
{
    public interface IDonorDal
    {
        Task<List<DonorDTO>> GetAll();
        Task<List<DonorDTO>> GetByFilter(string? name, string? email, string? giftName);
        Task Add(DonorDTO donor);
        Task Update(DonorDTO donor);
        Task Delete(int id);
    }
}