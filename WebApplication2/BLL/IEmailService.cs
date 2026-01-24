using System.Threading.Tasks;

namespace WebApplication2.BLL
{
    public interface IEmailService
    {
        Task SendWinnerNotificationAsync(string email, string userName, string giftName);
    }
}