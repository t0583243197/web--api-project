using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Models;
using WebApplication2.Models.DTO;

namespace WebApplication2.BLL
{
    public class RaffleSarviceBLL:IRaffleBLL
    {
        private readonly StoreContext _Storecontext;
        private readonly IWinnerDAL _winnerDal;
        private readonly IEmailService _emailService;
        private readonly ILogger<RaffleSarviceBLL> _logger;
       
        public RaffleSarviceBLL(
            StoreContext context, 
            IWinnerDAL winnerDal, 
            IEmailService emailService,
            ILogger<RaffleSarviceBLL> logger)
        {
            _Storecontext = context;
            _winnerDal = winnerDal;
            _emailService = emailService;
            _logger = logger;
        }
        
        public async Task<WinnerModel> RunRaffle(int giftId)
        {
            _logger.LogInformation("התחילה הגרלה עבור מתנה {GiftId}", giftId);

            var existingWinner = await _winnerDal.IsGiftAlreadyWonAsync(giftId);
            if (existingWinner)
            {
                _logger.LogWarning("מתנה {GiftId} כבר הוגרלה", giftId);
                throw new Exception("מתנה זו כבר הוגרלה ויש לה זוכה");
            }

            {
                var tickets = await _Storecontext.OrderTicket
          .Where(ot => ot.GiftId == giftId && ot.Order.IsDraft == false)
          .Select(ot => new
          {
              UserId = ot.Order.UserId,
              Quantity = ot.Quantity
          })
          .ToListAsync();

                _logger.LogInformation("נמצאו {TicketCount} כרטיסים עבור מתנה {GiftId}", 
                    tickets.Count, giftId);

                if (!tickets.Any())
                {
                    _logger.LogWarning("אין כרטיסים למתנה {GiftId} - ההגרלה בוטלה", giftId);
                    return null;
                }

                List<int> rafflePool = new List<int>();

                foreach (var ticket in tickets)
                {
                    for (int i = 0; i < ticket.Quantity; i++)
                    {
                        rafflePool.Add(ticket.UserId);
                    }
                }

                Random rnd = new Random();
                int winnerIndex = rnd.Next(rafflePool.Count);
                int winnerUserId = rafflePool[winnerIndex];

                _logger.LogInformation(
                    "בחור זוכה: UserId={WinnerUserId} בעמדה {Index} מתוך {TotalCount}", 
                    winnerUserId, winnerIndex, rafflePool.Count);

                var winner = new WinnerModel
                {
                    GiftId = giftId,
                    UserId = winnerUserId
                };

                try
                {
                    await _winnerDal.AddWinner(winner);
                    _logger.LogInformation(
                        "הזוכה נשמר בהצלחה - GiftId={GiftId}, UserId={UserId}", 
                        giftId, winnerUserId);
                    
                    winner = await _Storecontext.Winners
                        .Include(w => w.User)
                        .Include(w => w.Gift)
                        .FirstOrDefaultAsync(w => w.GiftId == giftId && w.UserId == winnerUserId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, 
                        "שגיאה בשמירת הזוכה - GiftId={GiftId}, UserId={UserId}", 
                        giftId, winnerUserId);
                    throw;
                }
               
                try
                {
                    var user = await _Storecontext.Users.FindAsync(winnerUserId);
                    var gift = await _Storecontext.Gifts.FindAsync(giftId);
                    
                    if (user != null && gift != null)
                    {
                        await _emailService.SendWinnerNotificationAsync(user.Email, user.Name, gift.Name);
                        _logger.LogInformation(
                            "מייל זכייה נשלח בהצלחה - Email={Email}, Gift={GiftName}", 
                            user.Email, gift.Name);
                    }
                    else
                    {
                        _logger.LogWarning(
                            "לא ניתן לשלוח מייל - User או Gift לא נמצא. UserId={UserId}, GiftId={GiftId}", 
                            winnerUserId, giftId);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, 
                        "שגיאה בשליחת מייל זכייה ל-UserId={UserId}", winnerUserId);
                }

                _logger.LogInformation("הגרלה הושלמה בהצלחה - Winner UserId={UserId}", winnerUserId);
                return winner;
               


            }
        }
    }
}
