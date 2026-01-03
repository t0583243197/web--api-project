using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApplication2.DAL;
using WebApplication2.Models.DTO;

namespace WebApplication2.BLL
{
    public class DonorServiceBLL : IDonorBLL
    {
        private readonly IDonorDal _donorDal;

        public DonorServiceBLL(IDonorDal donorDal) => _donorDal = donorDal;

        //  הספרייה Task
        // לא מנהלת באופן אוטומטי את כל הנושא של האסינכרוניות או מחליטה בעצמה איזה קטעי קוד צריכים להמתין ואיזה לא.
        // תפקידה של הספרייה הוא לספק את הכלים כדי לבצע פעולות אסינכרוניות בצורה נוחה ויעילה.

        //החלטה אם עליך להשתמש ב-
        //await
        //או לא תלויה בך – כלומר, אתה זה שמנהל את הזרימה של התוכנית וההמתנה למשימות אסינכרוניות.

        //איך Task עובד?

        //Task הוא אובייקט שמייצג פעולה שצריכה להתבצע.
        //הוא לא מבצע את הפעולה בעצמו, אלא הוא אומר לך שהפעולה הזו התבצעה או מתבצעת.

        //אם אתה יוצר Task מבלי להשתמש ב- await, המשימה תתבצע ברקע, אך התוכנית לא תחכה לה ותמשיך עם הקוד הבא.
        //זה יכול להוביל לבעיות אם אתה מנסה להשתמש בתוצאה של המשימה לפני שהיא הושלמה.

        //אם אתה משתמש ב- await, התוכנית מחכה שהתוצאה של המשימה תחזור לפני שהיא ממשיכה עם הקוד הבא. ה- await זה שמסביר לתוכנית מתי לחכות ומתי לא.

        //מה תפקיד הספרייה Task?

        //הספרייה Task מספקת את הכלים שמאפשרים ליצור פעולות אסינכרוניות,
        //והיא מטפלת בניהול התהליכים (threads) שמתבצעים ברקע.היא לא מחליטה אוטומטית אם עליך לחכות למשימה או לא,
        //אלא משאירה לך את ההחלטה על כך.


//  
//=
        public  async Task<List<DonorDTO>> GetAllDonorsAsync() =>await _donorDal.GetAll();


        public Task<List<DonorDTO>> GetDonorsByFilterAsync(string? name, string? email, string? giftName)
            => _donorDal.GetByFilter(name, email, giftName);

        public Task AddDonorAsync(DonorDTO donor) => _donorDal.Add(donor);

        public Task UpdateDonorAsync(DonorDTO donor) => _donorDal.Update(donor);

        public Task DeleteDonorAsync(int id) => _donorDal.Delete(id);
    }



}