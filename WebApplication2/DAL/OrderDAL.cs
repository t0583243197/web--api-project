using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Models.DTO;

namespace WebApplication2.DAL
{
    public class OrderDAL : IOrderDal
    {
        private readonly StoreContext _context;

        public OrderDAL(StoreContext context)
        {
            _context = context;
        }


        public int AddOrder(OrderModel order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges(); // שומר את ההזמנה ואת כל ה-OrderItems שקשורים אליה
            return order.Id;
        }

        public List<PurchaserDetailsDto> GetPurchasersByGiftId(int giftId)
        {
            return _context.OrderTicket
                .Where(t => t.GiftId == giftId && t.Order.IsDraft == true) // סינון: רק הזמנות סגורות
                .Select(t => new PurchaserDetailsDto
                {
                    CustomerName = t.Order.User.Name,
                    Email = t.Order.User.Email,
                    TicketsCount = t.Quantity
                })
                .ToList();
        }
        public List<OrderModel> GetUserOrders(int userId)
        {
            return _context.Orders
                .Include(o => o.OrderItems) // השורה הזו היא הקסם שחסר! היא טוענת את הכרטיסים
                .Where(o => o.UserId == userId && o.IsDraft == true)
                .ToList();
        }
    }
}