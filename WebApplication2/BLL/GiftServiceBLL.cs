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

        public async Task<List<GiftDTO>> GetAllGiftsAsync()
        {
            var gifts = await _giftDal.GetAll();
            return gifts;
        }

        public async Task<List<GiftDTO>> GetGiftsByFilterAsync(string? name, string? donorName, int? minPurchasers)
        {
            var gifts = await _giftDal.GetByFilter(name, donorName, minPurchasers);
            return gifts;
        }

        public Task<List<GiftDTO>> GetGiftsSortedByPriceAsync() => _giftDal.GetGiftsSortedByPrice();

        public Task<List<GiftDTO>> GetMostPurchasedGiftsAsync() => _giftDal.GetMostPurchasedGifts();

        public Task AddGiftAsync(GiftDTO gift) => _giftDal.Add(gift);

        public Task UpdateGiftAsync(GiftDTO gift) => _giftDal.Update(gift);

        public async Task DeleteGiftAsync(int id)
        {
            // בדוק אם קיימות רכישות מאושרות (לא טיוטה) למתנה זו
            bool hasConfirmedOrders = await _orderDal.HasConfirmedOrdersForGift(id);

            if (hasConfirmedOrders)
                throw new BusinessException("לא ניתן למחוק את המתנה כיוון שכבר יש עבורה רכישות מאושרות שלא ניתן להפר.");

            await _giftDal.Delete(id);
        }

        public async Task<SalesSummaryDto> GetSalesSummaryAsync()
        {
            var totalRevenue = await _giftDal.GetTotalSalesAsync();
            var totalOrders = await _orderDal.GetConfirmedOrdersCountAsync();
            var totalTickets = await _orderDal.GetTotalTicketsSoldAsync();
            
            return new SalesSummaryDto
            {
                TotalRevenue = totalRevenue,
                TotalOrders = totalOrders,
                TotalTicketsSold = totalTickets,
                SalesPerGift = new List<GiftSalesDto>()
            };
        }

        public Task<List<GiftWinnerDto>> GetGiftsWithWinnersAsync() => _giftDal.GetGiftsWithWinnersAsync();
    }
}