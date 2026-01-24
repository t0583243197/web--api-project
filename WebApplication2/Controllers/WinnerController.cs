using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Models;
using WebApplication2.BLL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
// controller for all the crud actions on winner
namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WinnerController : ControllerBase
    {
        private readonly WinnerDal _winnerDal;
        private readonly IEmailService _emailService;
        
        public WinnerController(WinnerDal winnerDal, IEmailService emailService) {
            _winnerDal = winnerDal;
            _emailService = emailService;
        }
        // GET: api/winner
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task <List<WinnerModel>> Get()   
        {
            return  _winnerDal.GetAllWinners();
        }

        // GET api/winner/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<WinnerModel> Get(int id)
        {
            return await _winnerDal.WinnerBYId(id);
        }

        // POST api/winner
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Post([FromBody] WinnerModel winnerModel)
        {
            try
            {
                await _winnerDal.AddWinner(winnerModel);
                
                // שליחת מייל אוטומטית לזוכה
                var winner = await _winnerDal.WinnerBYId(winnerModel.Id);
                if (winner != null)
                {
                    await _emailService.SendWinnerNotificationAsync(
                        winner.User.Email,
                        winner.User.Name,
                        winner.Gift.Name
                    );
                }
                
                return Ok("זוכה נוסף בהצלחה ומייל נשלח");
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"שגיאה: {ex.Message}");
            }
        }
  

        // PUT api/<ValuesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // POST api/winner/send-email/{winnerId}
        [HttpPost("send-email/{winnerId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> SendWinnerEmail(int winnerId)
        {
            try
            {
                var winner = await _winnerDal.WinnerBYId(winnerId);
                if (winner == null)
                {
                    return NotFound("זוכה לא נמצא");
                }

                await _emailService.SendWinnerNotificationAsync(
                    winner.User.Email, 
                    winner.User.Name, 
                    winner.Gift.Name
                );

                return Ok("מייל נשלח בהצלחה");
            }
            catch (Exception ex)
            {
                return BadRequest($"שגיאה בשליחת מייל: {ex.Message}");
            }
        }

        // DELETE api/winner/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task Delete(int id)
        {
            await _winnerDal.DeleteWinner(id);
            
        }
    }
}
