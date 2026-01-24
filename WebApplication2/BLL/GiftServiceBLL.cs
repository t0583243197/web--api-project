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

        public Task<List<GiftDTO>> GetAllGiftsAsync() => _giftDal.GetAll();

        public Task<List<GiftDTO>> GetGiftsByFilterAsync(string? name, string? donorName, int? minPurchasers)
            => _giftDal.GetByFilter(name, donorName, minPurchasers);

        public Task<List<GiftDTO>> GetGiftsSortedByPriceAsync() => _giftDal.GetGiftsSortedByPrice();

        public Task<List<GiftDTO>> GetMostPurchasedGiftsAsync() => _giftDal.GetMostPurchasedGifts();

        public Task AddGiftAsync(GiftDTO gift) => _giftDal.Add(gift);

        public Task UpdateGiftAsync(GiftDTO gift) => _giftDal.Update(gift);

        public async Task DeleteGiftAsync(int id)
        {
            // Check if there are orders for this gift; if yes, prevent deletion.
            bool hasOrders = await _orderDal.HasOrdersForGift(id);

            if (hasOrders)
                throw new BusinessException("לא ניתן למחוק את המתנה כיוון שכבר נרכשו עבורה כרטיסים.");

            await _giftDal.Delete(id);
        }

        public async Task<SalesSummaryDto> GetSalesSummaryAsync()
        {
            var totalRevenue = await _giftDal.GetTotalSalesAsync();
            return new SalesSummaryDto
            {
                TotalRevenue = totalRevenue,
                TotalTicketsSold = 0,
                SalesPerGift = new List<GiftSalesDto>()
            };
        }

        public Task<List<GiftWinnerDto>> GetGiftsWithWinnersAsync() => _giftDal.GetGiftsWithWinnersAsync();
    }
}