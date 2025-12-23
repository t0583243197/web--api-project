using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.BLL;
using WebApplication2.Models.DTO;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // כאן נרצה שכל משתמש מחובר יוכל לבצע הזמנה (לא רק מנהל)
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderBLL _orderBll;

        public OrderController(IOrderBLL orderBll)
        {
            _orderBll = orderBll;
        }

        [HttpPost("checkout")]
        public IActionResult Checkout([FromBody] OrderDTO orderDto)
        {
            if (orderDto == null || orderDto.Items.Count == 0)
            {
                return BadRequest("סל הקניות ריק");
            }

            try
            {
                int orderId = _orderBll.PlaceOrder(orderDto);
                return Ok(new { Message = "ההזמנה בוצעה בהצלחה!", OrderId = orderId });
            }
            catch (Exception ex)
            {
                // כאן המידלוור של השגיאות יתפוס שגיאות לא צפויות
                return StatusCode(500, "אירעה שגיאה בעיבוד ההזמנה");
            }
        }
    }
}