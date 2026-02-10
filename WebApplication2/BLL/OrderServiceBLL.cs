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
        private readonly IWinnerDAL _winnerDal;
        private readonly IMapper _mapper; // <-- Add this line

        public OrderServiceBLL(IOrderDal orderDal, IGiftDal giftDal, IWinnerDAL winnerDal, IMapper mapper) // <-- Add IWinnerDAL to constructor
        {
            _orderDal = orderDal;
            _giftDal = giftDal;
            _winnerDal = winnerDal;
            _mapper = mapper; // <-- Assign mapper
        }

        public async Task<List<PurchaserDetailsDto>> GetPurchasersForGiftAsync(int giftId)
        {
            // אם IOrderDal.GetPurchasersByGiftId מחזיר Task, יש להמתין לו
            return await _orderDal.GetPurchasersByGiftId(giftId);
        }

        public async Task<int> PlaceOrderAsync(OrderDTO Dto)
        {
            // בדיקה שאף מתנה לא הוגרלה כבר
            foreach (var itemDto in Dto.OrderItems)
            {
                bool isAlreadyWon = await _winnerDal.IsGiftAlreadyWonAsync(itemDto.GiftId);
                if (isAlreadyWon)
                {
                    throw new BusinessException("לא ניתן לרכוש כרטיסים למתנה שכבר הוגרלה");
                }
            }

            decimal totalSum = 0m;

            var orderTickets = new List<OrderTicketModel>();

            // שליפת כל המתנות בצורה אסינכרונית כדי לקבל מחירים עדכניים
            var gifts = await _giftDal.GetAll();

            foreach (var itemDto in Dto.OrderItems)
            {
                var gift = gifts.FirstOrDefault(g => g.Id == itemDto.GiftId);
                if (gift != null)
                {
                    totalSum += gift.TicketPrice * itemDto.Quantity;

                    orderTickets.Add(new OrderTicketModel
                    {
                        GiftId = gift.Id,
                        Quantity = itemDto.Quantity
                    });
                }
            }

            var newOrder = new OrderModel
            {
                UserId = Dto.UserId,
                OrderDate = DateTime.Now,
                TotalAmount = (double)totalSum,
                IsDraft = Dto.IsDraft,
                OrderItems = orderTickets
            };

            // אם IOrderDal.AddOrder הוא סינכרוני – נשמור קריאה סינכרונית (אפשר לעדכן ל‑AddOrderAsync מאוחר יותר)
            var orderId = await Task.Run(() => _orderDal.AddOrder(newOrder));
            return orderId; // <-- Fixed: return int directly, not Task<int>
        }

        public async Task<List<OrderDTO>> GetUserHistoryAsync(int userId)
        {
            var orders = await _orderDal.GetUserOrders(userId);
            if (orders == null || !orders.Any()) return new List<OrderDTO>();
            return _mapper.Map<List<OrderDTO>>(orders);
        }

        public async Task<List<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _orderDal.GetAllOrders();
            if (orders == null || !orders.Any()) return new List<OrderDTO>();
            return _mapper.Map<List<OrderDTO>>(orders);
        }

        public async Task ConfirmOrderAsync(int orderId)
        {
            bool success = await _orderDal.ConfirmOrderAsync(orderId);
            if (!success)
            {
                throw new BusinessException("הזמנה לא נמצאה");
            }
        }

        public async Task RemoveOrderItemAsync(int orderId, int giftId)
        {
            var order = await _orderDal.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new BusinessException("הזמנה לא נמצאה");
            }

            if (!order.IsDraft)
            {
                throw new BusinessException("לא ניתן לשנות הזמנה לאחר רכישה");
            }

            bool success = await _orderDal.RemoveOrderItemAsync(orderId, giftId);
            if (!success)
            {
                throw new BusinessException("פריט לא נמצא בהזמנה");
            }
        }

        public async Task AddItemToOrderAsync(int orderId, int giftId, int quantity)
        {
            var order = await _orderDal.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new BusinessException("הזמנה לא נמצאה");
            }

            if (!order.IsDraft)
            {
                throw new BusinessException("לא ניתן לשנות הזמנה מאושרת");
            }

            await _orderDal.AddItemToOrderAsync(orderId, giftId, quantity);
        }
    }
}