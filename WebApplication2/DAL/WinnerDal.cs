using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using WebApplication2.Models;
using WebApplication2.Models.DTO;

namespace WebApplication2.DAL
{
    public class WinnerDal : IWinnerDAL
    {
        private StoreContext _context;
        private IMapper _mapper;
        private ILogger _logger;
        public WinnerDal(StoreContext context,IMapper mapper,ILogger logger)
        {
            this._context = context;
            this._mapper = mapper;
            this._logger = logger;
        }
        
        public async Task AddWinner(WinnerModel winnerModel)
        {
            //var WinnerModel = _mapper.Map<WinnerModel>(winnerDTO); // המרת DTO ל-Model
            try
            {
                _context.Winners.Add(winnerModel); // הוספה ל-DbSet
                _context.SaveChangesAsync(); // שמירה למסד הנתונים
            }
            catch
            {
                _logger.LogError("אירעה שגיאה תוך כדי הוספת זוכה עם מזהה {UserId}", winnerModel.UserId);
                throw new Exception();
            }
        }
        public async  Task  DeleteWinner(int userId)
        {
            try
            {
                var winner1 = await _context.Winners.FindAsync(userId);
            
                if (winner1 != null)
                {
                    _context.Winners.Remove(winner1);
                    _ = _context.SaveChangesAsync();
                }
            }
            catch
            {
                _logger.LogError("אירעה שגיאה תוך כדי מחיקת זוכה עם מזהה {UserId}", userId);
                throw new Exception() ; 
            }

        }

        public List<WinnerModel> GetAllWinners()
        {
            try
            {
             var winners= _context.Winners.ToList();
                return winners;

            }
            catch
            {
                _logger.LogError("אירעה שגיאה תוך כדי שליפת כל הזוכים");
                throw new Exception();

            }
        }

        //public void UpdateWinner(WinnerModel winner)
        //{
        //    throw new NotImplementedException();
        //}

        public async  Task<WinnerModel> WinnerBYId(int userId)
        {
             try
            {
               var winner= await _context.Winners.FindAsync(userId);
                if (winner == null)
                {
                    _logger.LogWarning("חיפוש נכשל: לא נמצא זוכה עם מזהה {UserId}", userId);
                }
                return winner;
            }
            catch 
            {
                _logger.LogError(" אירעה שגיאה תוך כדי חיפוש מנצח אחד על ידי התז שלו {UserId}", userId);

                throw new Exception();
                   
            }
        
        }
    }
}
