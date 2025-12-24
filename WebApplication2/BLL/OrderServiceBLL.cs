using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.DAL;
using WebApplication2.Models;
using WebApplication2.Models.DTO;

namespace WebApplication2.BLL
{
    public class OrderServiceBLL : IOrderBLL
    {
        private readonly IOrderDal _orderDal;
        private readonly IGiftDal _giftDal;

        public OrderServiceBLL(IOrderDal orderDal, IGiftDal giftDal)
        {
            _orderDal = orderDal;
            _giftDal = giftDal;
        }

        public Task<List<PurchaserDetailsDto>> GetPurchasersForGiftAsync(int giftId)
        {
            // אם IOrderDal אינו אסינכרוני, עטיפה ב-Task.FromResult לשמירה על API אסינכרוני
            var result = _orderDal.GetPurchasersByGiftId(giftId);
            return Task.FromResult(result);
        }

        public async Task<int> PlaceOrderAsync(OrderDTO Dto)
        {
            decimal totalSum = 0m;

            var orderTickets = new List<OrderTicketModel>();

            // שליפת כל המתנות בצורה אסינכרונית כדי לקבל מחירים עדכניים
            var gifts = await _giftDal.GetAllAsync();

            foreach (var itemDto in Dto.Items)
            {
                var gift = gifts.FirstOrDefault(g => g.Id == itemDto.GiftId);
                if (gift != null)
                {
                    totalSum += gift.TicketPrice;

                    orderTickets.Add(new OrderTicketModel
                    {
                        GiftId = gift.Id,
                        Quantity = 1
                    });
                }
            }

            var newOrder = new OrderModel
            {
                UserId = Dto.UserId,
                OrderDate = DateTime.Now,
                TotalAmount = (double)totalSum, // Explicit cast from decimal to double
                OrderItems = orderTickets
            };

            // אם IOrderDal.AddOrder הוא סינכרוני – נשמור קריאה סינכרונית (אפשר לעדכן ל‑AddOrderAsync מאוחר יותר)
            var orderId = _orderDal.AddOrder(newOrder);
            return orderId;
        }

        public Task<List<OrderDTO>> GetUserHistoryAsync(int userId)
        {
            // מימוש דמה לעת עתה
            return Task.FromResult(new List<OrderDTO>());
        }
    }
}