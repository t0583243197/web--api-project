using WebApplication2.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication2.DAL
{
    public interface IGiftDal
    {
       public Task<List<GiftDTO>> GetAllAsync();
        Task<List<GiftDTO>> GetByFilterAsync(string? name, string? donorName, int? minPurchasers);
        Task<List<GiftDTO>> GetGiftsSortedByPriceAsync();
        Task<List<GiftDTO>> GetMostPurchasedGiftsAsync();
        Task AddAsync(GiftDTO gift);
        Task UpdateAsync(GiftDTO gift);
        Task DeleteAsync(int id);
    }
}