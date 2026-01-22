using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models.DTO;

namespace WebApplication2.BLL
{
    public interface IOrderBLL
    {
        Task<int> PlaceOrderAsync(OrderDTO dto);
        Task<List<PurchaserDetailsDto>> GetPurchasersForGiftAsync(int giftId);
        Task<List<OrderDTO>> GetUserHistoryAsync(int userId);
        Task ConfirmOrderAsync(int orderId);
        Task RemoveOrderItemAsync(int orderId, int giftId);
    }
}