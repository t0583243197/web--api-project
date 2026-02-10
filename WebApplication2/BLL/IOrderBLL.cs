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
        Task<List<OrderDTO>> GetAllOrdersAsync();
        Task ConfirmOrderAsync(int orderId);
        Task RemoveOrderItemAsync(int orderId, int giftId);
        Task AddItemToOrderAsync(int orderId, int giftId, int quantity);
    }
}