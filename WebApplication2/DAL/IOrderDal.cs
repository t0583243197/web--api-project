using WebApplication2.Models;

namespace WebApplication2.DAL
{
    public interface IOrderDal
    {
        int AddOrder(WebApplication2.Models.OrderTicketModel order); // Fully qualify if needed
        List<WebApplication2.Models.OrderTicketModel> GetUserOrders(int userId);
    }
}