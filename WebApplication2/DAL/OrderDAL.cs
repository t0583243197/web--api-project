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


        public int AddOrder(OrderModel order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges(); // שומר את ההזמנה ואת כל ה-OrderItems שקשורים אליה
            return order.Id;
        }

        public List<OrderModel> GetUserOrders(int userId)
        {
            return _context.Orders
                .Where(o => o.UserId == userId)

                .ToList();
        }
    }
}