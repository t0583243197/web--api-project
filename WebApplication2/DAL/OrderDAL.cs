using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.Models.DTO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication2.DAL
{
    public class OrderDAL : IOrderDal
    {
        private readonly StoreContext _context;

        public OrderDAL(StoreContext context)
        {
            _context = context;
        }

        public async Task<int> AddOrder(OrderModel order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); // saves the order and its items
            return order.Id;
        }

        // Read-only: AsNoTracking + projection
        public async Task<List<PurchaserDetailsDto>> GetPurchasersByGiftId(int giftId)
        {
            return await _context.OrderTicket
                .AsNoTracking()
                .Where(t => t.GiftId == giftId && t.Order.IsDraft == true)
                .Select(t => new PurchaserDetailsDto
                {
                    CustomerName = t.Order.User.Name,
                    Email = t.Order.User.Email,
                    TicketsCount = t.Quantity
                })
                .ToListAsync();
        }

        // Read-only: AsNoTracking; project orders with items to avoid loading other columns unnecessarily
        public async Task<List<OrderModel>> GetUserOrders(int userId)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(o => o.UserId == userId && o.IsDraft == true)
                .Select(o => new OrderModel
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    IsDraft = o.IsDraft,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    OrderItems = o.OrderItems.Select(oi => new OrderTicketModel
                    {
                        Id = oi.Id,
                        OrderId = oi.OrderId,
                        GiftId = oi.GiftId,
                        Quantity = oi.Quantity,
                        Gift = null // avoid loading full Gift; load separately if caller needs it
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<bool> HasOrdersForGift(int giftId)
        {
            return await _context.OrderTicket.AnyAsync(t => t.GiftId == giftId);
        }
    }
}