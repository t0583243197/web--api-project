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

        public int PlaceOrder(OrderDTO Dto)
        {
            double totalSum = 0;

            var orderTickets = new List<OrderTicketModel>();

            // מעבר על כל המתנות שהמשתמש בחר
            foreach (var itemDto in Dto.Items)
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

            var newOrder = new OrderModel
            {
                UserId = Dto.UserId,
                OrderDate = DateTime.Now,
                TotalAmount = totalSum,
                OrderItems = orderTickets
            };


            return _orderDal.AddOrder(newOrder);
        }

        public List<OrderDTO> GetUserHistory(int userId)
        {
            // כאן תבוא לوجיקה של שליפת היסטוריה ומיפוי ל-DTO
            return new List<OrderDTO>();
        }
    }
}