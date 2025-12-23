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

        public int PlaceOrder(OrderDTO orderDto)
        {
            double totalSum = 0;
            var orderTickets = new List<OrderTicketModel>();

            // מעבר על כל המתנות שהמשתמש בחר
            foreach (var itemDto in orderDto.Items)
            {
                // שליפת המתנה מה-DB כדי לוודא מחיר עדכני
                var gift = _giftDal.getAll().FirstOrDefault(g => g.Id == itemDto.GiftId);
                if (gift != null)
                {
                    totalSum += gift.TicketPrice;
                    orderTickets.Add(new OrderTicketModel
                    {
                        GiftId = gift.Id,
                        Quantity = 1 // בהגרלה סינית כל שורה היא כרטיס
                    });
                }
            }

            // יצירת מודל ההזמנה הסופי
            // var newOrder = new OrderModel
            // {
            //     UserId = orderDto.UserId,
            //     OrderDate = DateTime.Now,
            //     TotalAmount = totalSum,
            //     OrderTickets = orderTickets
            // };

            // Call AddOrder for each ticket
            int lastOrderId = 0;
            foreach (var ticket in orderTickets)
            {
                // Assuming AddOrder expects a OrderTicketModel, use that instead of TicketModel
                lastOrderId = _orderDal.AddOrder(ticket);
            }
            return lastOrderId;
        }

        public List<OrderDTO> GetUserHistory(int userId)
        {
            // כאן תבוא לוגיקה של שליפת היסטוריה ומיפוי ל-DTO
            return new List<OrderDTO>();
        }
    }
}