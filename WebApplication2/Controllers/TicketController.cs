using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApplication2.BLL;
using WebApplication2.Models.DTO;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderBLL _orderBll;

        public OrderController(IOrderBLL orderBll)
        {
            _orderBll = orderBll;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] OrderDTO orderDto)
        {
            if (orderDto == null || orderDto.Items == null || orderDto.Items.Count == 0)
            {
                return BadRequest("סל הקניות ריק");
            }

            try
            {
                int orderId = await _orderBll.PlaceOrderAsync(orderDto);
                return Ok(new { Message = "ההזמנה בוצעה בהצלחה!", OrderId = orderId });
            }
            catch (Exception)
            {
                return StatusCode(500, "אירעה שגיאה בעיבוד ההזמנה");
            }
        }

        [HttpGet("purchasers/{giftId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetPurchasers(int giftId)
        {
            var purchasers = await _orderBll.GetPurchasersForGiftAsync(giftId);
            if (purchasers == null || purchasers.Count == 0)
            {
                return NotFound("לא נמצאו רוכשים למתנה זו");
            }
            return Ok(purchasers);
        }

        [HttpGet("user/history/{userId}")]
        public async Task<IActionResult> GetUserOrderHistory(int userId)
        {
            try
            {
                var orders = await _orderBll.GetUserHistoryAsync(userId);
                if (orders == null || orders.Count == 0)
                {
                    return NotFound("לא נמצאו הזמנות עבור משתמש זה");
                }
                return Ok(orders);
            }
            catch (Exception)
            {
                return StatusCode(500, "אירעה שגיאה בעת fetch היסטוריית ההזמנה");
            }
        }
    }
}