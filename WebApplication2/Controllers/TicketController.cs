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
        [Authorize]
        public async Task<IActionResult> Checkout([FromBody] OrderDTO orderDto)
        {
            if (orderDto == null || orderDto.OrderItems == null || orderDto.OrderItems.Count == 0)
            {
                return BadRequest("סל הקניות ריק");
            }

            try
            {
                // קבלת ה-userId מהטוקן
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized("משתמש לא מזוהה");
                }

                orderDto.UserId = userId; // שימוש ב-userId מהטוקן
                int orderId = await _orderBll.PlaceOrderAsync(orderDto);
                return Ok(new { Message = "ההזמנה בוצעה בהצלחה!", OrderId = orderId });
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Checkout: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, $"אירעה שגיאה בעיבוד ההזמנה: {ex.Message}");
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

        [HttpGet("all")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderBll.GetAllOrdersAsync();
                return Ok(orders);
            }
            catch (Exception)
            {
                return StatusCode(500, "אירעה שגיאה בטעינת ההזמנות");
            }
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

        [HttpPost("confirm/{orderId}")]
        public async Task<IActionResult> ConfirmOrder(int orderId)
        {
            try
            {
                await _orderBll.ConfirmOrderAsync(orderId);
                return Ok("ההזמנה אושרה בהצלחה");
            }
            catch (BusinessException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "אירעה שגיאה באישור ההזמנה");
            }
        }

        [HttpPost("{orderId}/add-item")]
        [AllowAnonymous]
        public async Task<IActionResult> AddItemToOrder(int orderId, [FromBody] AddItemRequest request)
        {
            try
            {
                await _orderBll.AddItemToOrderAsync(orderId, request.GiftId, request.Quantity);
                return Ok("הפריט נוסף להזמנה");
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "אירעה שגיאה");
            }
        }
    }

    public class AddItemRequest
    {
        public int GiftId { get; set; }
        public int Quantity { get; set; }
    }
}