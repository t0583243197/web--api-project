using WebApplication2.DAL;
using WebApplication2.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GiftServiceBLL : IGiftBLL
{   
    private readonly IGiftDal _giftDal;

    public GiftServiceBLL(IGiftDal giftDal) => _giftDal = giftDal;

    public Task<List<GiftDTO>> GetAllGiftsAsync() => _giftDal.GetAllAsync();

    public Task<List<GiftDTO>> GetGiftsByFilterAsync(string? name, string? donorName, int? minPurchasers)
        => _giftDal.GetByFilterAsync(name, donorName, minPurchasers);

    public Task<List<GiftDTO>> GetGiftsSortedByPriceAsync() => _giftDal.GetGiftsSortedByPriceAsync();

    public Task<List<GiftDTO>> GetMostPurchasedGiftsAsync() => _giftDal.GetMostPurchasedGiftsAsync();

    public Task AddGiftAsync(GiftDTO gift) => _giftDal.AddAsync(gift);

    public Task UpdateGiftAsync(GiftDTO gift) => _giftDal.UpdateAsync(gift);

    public Task DeleteGiftAsync(int id) => _giftDal.DeleteAsync(id);
}