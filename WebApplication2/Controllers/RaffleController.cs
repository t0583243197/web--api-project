using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.BLL;
using WebApplication2.DAL;
using WebApplication2.Models;
using WebApplication2.Models.DTO;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RaffleController : ControllerBase
    {
        private readonly RaffleSarviceBLL _raffleSarviceBLL;
        
        public RaffleController(RaffleSarviceBLL raffleSarviceBLL)
        {
            _raffleSarviceBLL = raffleSarviceBLL;
        }
        
        [HttpPost("run/{giftId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> RunRaffle(int giftId)
        {
            try
            {
                var winner = await _raffleSarviceBLL.RunRaffle(giftId);
                return Ok(new { message = "הגרלה בוצעה בהצלחה", winner });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
