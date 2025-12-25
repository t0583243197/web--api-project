using WebApplication2.Models;
using WebApplication2.Models.DTO;

namespace WebApplication2.DAL
{
    public interface IOrderDal
    {

     
        int AddOrder(OrderModel ticket); // מחזיר את ה-ID של ההזמנה החדשה
        List<OrderModel> GetUserOrders(int userId);// מחזיר את כל ההזמנות של משתמש מסוים
        List<PurchaserDetailsDto> GetPurchasersByGiftId(int giftId);// מחזיר את כל הרוכשים של מתנה מסוימת
      Task<bool> HasOrdersForGiftAsync(int giftId);

    }
}