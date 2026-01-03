using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
// controller for all the crud actions on winner
namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WinnerController : ControllerBase
    {
        private readonly WinnerDal _winnerDal;
        public WinnerController(WinnerDal winnerDal) {
            _winnerDal = winnerDal;
                }
        // GET: api/<ValuesController>
        [HttpGet]
       
        public async Task <List<WinnerModel>> Get()   
        {
            return  _winnerDal.GetAllWinners();
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<WinnerModel> Get(int id)
        {
            return await _winnerDal.WinnerBYId(id);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task Post([FromBody] WinnerModel winnerModel)
        {
            await _winnerDal.AddWinner(winnerModel);
        }
  

        // PUT api/<ValuesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _winnerDal.DeleteWinner(id);
            
        }
    }
}
