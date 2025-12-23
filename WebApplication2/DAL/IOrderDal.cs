using WebApplication2.Models;

namespace WebApplication2.DAL
{
    public interface IOrderDal
    {

     
        int AddOrder(OrderModel ticket); // מחזיר את ה-ID של ההזמנה החדשה
        List<OrderModel> GetUserOrders(int userId);

    }
}