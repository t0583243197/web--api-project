using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.DAL;
using WebApplication2.Models.DTO;

namespace WebApplication2.BLL
{
    public class GiftServiceBLL : IGiftBLL
    {
        private readonly IGiftDal _giftDal;
        private readonly IOrderDal _orderDal;

        public GiftServiceBLL(IGiftDal giftDal, IOrderDal orderDal)
        {
            _giftDal = giftDal ?? throw new ArgumentNullException(nameof(giftDal));
            _orderDal = orderDal ?? throw new ArgumentNullException(nameof(orderDal));
        }

        public Task<List<GiftDTO>> GetAllGiftsAsync() => _giftDal.GetAllAsync();

        public Task<List<GiftDTO>> GetGiftsByFilterAsync(string? name, string? donorName, int? minPurchasers)
            => _giftDal.GetByFilterAsync(name, donorName, minPurchasers);

        public Task<List<GiftDTO>> GetGiftsSortedByPriceAsync() => _giftDal.GetGiftsSortedByPriceAsync();

        public Task<List<GiftDTO>> GetMostPurchasedGiftsAsync() => _giftDal.GetMostPurchasedGiftsAsync();

        public Task AddGiftAsync(GiftDTO gift) => _giftDal.AddAsync(gift);

        public Task UpdateGiftAsync(GiftDTO gift) => _giftDal.UpdateAsync(gift);

        public async Task DeleteGiftAsync(int id)
        {
            // Check if there are orders for this gift; if yes, prevent deletion.
            bool hasOrders = await _orderDal.HasOrdersForGiftAsync(id);

            if (hasOrders)
                throw new BusinessException("לא ניתן למחוק את המתנה כיוון שכבר נרכשו עבורה כרטיסים.");

            await _giftDal.DeleteAsync(id);
        }
    }

    // Business exception
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message) {}
    }
}