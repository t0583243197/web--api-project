using WebApplication2.Models;

namespace WebApplication2.DAL
{
    public class OrderDAL : IOrderDal
    {
        private readonly StoreContext _context;

        public OrderDAL(StoreContext context)
        {
            _context = context;
        }

        public int AddOrder(OrderTicketModel orderTicket)
        {
            // Create a new OrderModel and add the ticket to it
            var order = new OrderModel
            {
                UserId = orderTicket.Order?.UserId ?? 0,
                OrderDate = DateTime.Now,
                TotalAmount = 0, // Set as needed
                OrderItems = new List<OrderTicketModel> { orderTicket } // Fixed property name
            };

            _context.Orders.Add(order);
            _context.SaveChanges();
            return order.Id;
        }

        public List<OrderTicketModel> GetUserOrders(int userId)
        {
            return _context.Orders
                .Where(o => o.UserId == userId)
                .SelectMany(o => o.OrderItems) // Fixed property name
                .ToList();
        }
    }
}