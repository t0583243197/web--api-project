using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.BLL;
using WebApplication2.DAL;
using WebApplication2.Models;
using WebApplication2.Models.DTO;

namespace WebApplication2.Controllers
{
    public class RaffleController : Controller
    {
        private readonly StoreContext _context;
        private readonly RaffleSarviceBLL _raffleSarviceBLL;
        public RaffleController(StoreContext context, RaffleSarviceBLL raffleSarviceBLL)
        {
            _context = context;
            _raffleSarviceBLL = raffleSarviceBLL;
        }
      public async Task <WinnerModel> RunRaffle(int giftId)
        {
            // יצירת מופע של RaffleSarviceBLL
            
            // קריאה למתודת ההגרלה
            var winner = await _raffleSarviceBLL.RunRaffle(giftId); 
            return winner;
        }
    }
}
