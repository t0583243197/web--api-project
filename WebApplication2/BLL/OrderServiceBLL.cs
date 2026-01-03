using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebApplication2.DAL;
using WebApplication2.Models;
using WebApplication2.Models.DTO;

namespace WebApplication2.BLL
{
    public class OrderServiceBLL : IOrderBLL
    {
        private readonly IOrderDal _orderDal;
        private readonly IGiftDal _giftDal;
        private readonly IMapper _mapper; // <-- Add this line

        public OrderServiceBLL(IOrderDal orderDal, IGiftDal giftDal, IMapper mapper) // <-- Add IMapper to constructor
        {
            _orderDal = orderDal;
            _giftDal = giftDal;
            _mapper = mapper; // <-- Assign mapper
        }

        public async Task<List<PurchaserDetailsDto>> GetPurchasersForGiftAsync(int giftId)
        {
            // אם IOrderDal.GetPurchasersByGiftId מחזיר Task, יש להמתין לו
            return await _orderDal.GetPurchasersByGiftId(giftId);
        }

        public async Task<int> PlaceOrderAsync(OrderDTO Dto)
        {
            decimal totalSum = 0m;

            var orderTickets = new List<OrderTicketModel>();

            // שליפת כל המתנות בצורה אסינכרונית כדי לקבל מחירים עדכניים
            var gifts = await _giftDal.GetAll();

            foreach (var itemDto in Dto.OrderItems)
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
            var orderId = await Task.Run(() => _orderDal.AddOrder(newOrder));
            return orderId; // <-- Fixed: return int directly, not Task<int>
        }

        public async Task<List<OrderDTO>> GetUserHistoryAsync(int userId)
        {
            // שליפת הנתונים מה-DAL
            var orders = await _orderDal.GetUserOrders(userId);

            if (orders == null || !orders.Any()) return new List<OrderDTO>();

            // מיפוי הנתונים ל-DTO כדי שה-Controller יוכל להחזיר אותם
            return _mapper.Map<List<OrderDTO>>(orders);
        }
    }
}