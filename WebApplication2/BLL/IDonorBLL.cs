using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models.DTO;

namespace WebApplication2.BLL
{
    public interface IDonorBLL
    {
        Task<List<DonorDTO>> GetAllDonorsAsync();
        Task<List<DonorDTO>> GetDonorsByFilterAsync(string? name, string? email, string? giftName);
        Task AddDonorAsync(DonorDTO donor);
        Task UpdateDonorAsync(DonorDTO donor);
        Task DeleteDonorAsync(int id);
    }
}