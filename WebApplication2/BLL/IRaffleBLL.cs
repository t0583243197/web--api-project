using WebApplication2.Models;

namespace WebApplication2.BLL
{
    public interface IRaffleBLL
    {
        public Task<WinnerModel> RunRaffle(int giftId);
    }
}
