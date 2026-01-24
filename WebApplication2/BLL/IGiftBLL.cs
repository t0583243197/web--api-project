using WebApplication2.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IGiftBLL
{
    Task<List<GiftDTO>> GetAllGiftsAsync();
    Task<List<GiftDTO>> GetGiftsByFilterAsync(string? name, string? donorName, int? minPurchasers);
    Task<List<GiftDTO>> GetGiftsSortedByPriceAsync();
    Task<List<GiftDTO>> GetMostPurchasedGiftsAsync();
    Task AddGiftAsync(GiftDTO gift);
    Task UpdateGiftAsync(GiftDTO gift);
    Task DeleteGiftAsync(int id);
    Task<SalesSummaryDto> GetSalesSummaryAsync();
    Task<List<GiftWinnerDto>> GetGiftsWithWinnersAsync();
}