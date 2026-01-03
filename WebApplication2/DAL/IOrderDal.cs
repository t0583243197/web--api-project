using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Models.DTO;

namespace WebApplication2.DAL
{
    public interface IOrderDal
    {
        Task<int> AddOrder(OrderModel order);
        Task<List<PurchaserDetailsDto>> GetPurchasersByGiftId(int giftId);
        Task<List<OrderModel>> GetUserOrders(int userId);
        Task<bool> HasOrdersForGift(int giftId);
    }
}