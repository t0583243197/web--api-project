using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Models;
using WebApplication2.Models.DTO;

namespace WebApplication2.BLL
{
    public class RaffleSarviceBLL:IRaffleBLL//מבצעת את ההגרלה מחלקה
    {
        private readonly StoreContext _Storecontext; // זה המופע (Instance)
        private readonly IWinnerDAL _winnerDal;
        private readonly IEmailService _emailService;
       
        //bll  עשיית ההגרלה
        //בינה-להפריד בין שכבת המנצח ללוגיקה של עשיית ההגרלה
        public RaffleSarviceBLL(StoreContext context, IWinnerDAL winnerDal, IEmailService emailService) // ההזרקה קורה כאן
        {
            _Storecontext = context;
            _winnerDal = winnerDal;
            _emailService = emailService;
        }
        
        
        public async Task<WinnerModel> RunRaffle(int giftId)// מקבל id
                                                            // של מתנה ומחזיר את הזוכה  
        {
            // --- שלב 1: איסוף כל הכרטיסים שנמכרו למתנה הזו --- 
            // שליפת כל כרטיסי ההזמנה (OrderTicket) עבור המתנה הנתונה (giftId)
            //  שימוש select
            // כדי לבצע join
            //בין טבלת כרטיסי ההזמנה וטבלת ההזמנות
            //יותר יעיל מאשר
            //include
            // רק כרטיסים שההזמנה שלהם לא טיוטה (IsDraft == false)
            // בחירת UserId וכמות הכרטיסים (Quantity) לכל משתמש
            {
                var tickets = await _Storecontext.OrderTicket
            
          .Where(ot => ot.GiftId == giftId && ot.Order.IsDraft == false)
          .Select(ot => new
          {
              UserId = ot.Order.UserId,
              Quantity = ot.Quantity
          })
          .ToListAsync();
                // בדיקת בטיחות: אם אף אחד לא קנה כרטיס למתנה הזו, אין את מי להגריל.
                if (!tickets.Any()) return null;

                // --- שלב 2: בניית "תיבת ההגרלה" (The Pool) ---
                // כאן אנחנו יוצרים רשימה של מספרי ID של משתמשים.
                List<int> rafflePool = new List<int>();

                foreach (var ticket in tickets)
                {
                    // זו הלוגיקה החשובה ביותר:
                    // נניח שיוסי (UserId 10)
                    // קנה 3 כרטיסים (Quantity = 3).
                    // הלולאה הזו תוסיף את המספר 10 לרשימה שלוש פעמים.
                    // ככה ליוסי יהיו 3 "פתקים" בתוך תיבת ההגרלה הווירטואלית שלנו.
                    for (int i = 0; i < ticket.Quantity; i++)
                    {
                        rafflePool.Add(ticket.UserId); // ticket
                                                       // אוביקט  מסוג
                                                       // orderTicket
                                                       // userId הוא השדה שהוגדר למעלה
                                                       //   שמכיל את מזהה המשתמש שקנה את הכרטיס
                    }
                }

                // --- שלב 3: שליפת "פתק" מהתיבה ---
                // יצירת מחולל מספרים אקראיים.
                Random rnd = new Random();

                // בחירת מיקום אקראי ברשימה. 
                // אם ברשימה יש 100 איברים, הפונקציה תבחר מספר בין 0 ל-99.
                int winnerIndex = rnd.Next(rafflePool.Count);

                // שליפת ה-UserId שנמצא במיקום שנבחר.
                int winnerUserId = rafflePool[winnerIndex];

                // --- שלב 4: הכנת התוצאה ---
                // יוצרים אובייקט חדש של "זוכה" עם הפרטים שיצאו בהגרלה.
                var winner = new WinnerModel
                {
                    GiftId = giftId,
                    UserId = winnerUserId
                };
               
                // --- שלב 5: שליחת מייל לזוכה ---
                try
                {
                    var user = await _Storecontext.Users.FindAsync(winnerUserId);
                    var gift = await _Storecontext.Gifts.FindAsync(giftId);
                    
                    if (user != null && gift != null)
                    {
                        await _emailService.SendWinnerNotificationAsync(user.Email, user.Name, gift.Name);
                    }
                }
                catch (Exception ex)
                {
                    // לוג שגיאה אך לא מפשיל את ההגרלה
                    Console.WriteLine($"שגיאה בשליחת מייל: {ex.Message}");
                }

                return winner;
               


            }
        }
    }
}
