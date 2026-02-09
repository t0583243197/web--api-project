/// <summary>
/// דוגמה לשימוש ב-Middlewares בקונטרולר
/// 
/// כאשר זורקים Exception כלשהו בפונקציה זו,
/// ה-ExceptionHandlingMiddleware יתפוס אותו, יתרגום לסטטוס קוד מתאים ויחזיר JSON
/// </summary>

using Microsoft.AspNetCore.Mvc;
using WebApplication2.BLL;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExampleController : ControllerBase
    {
        private readonly IGiftBLL _giftService;
        private readonly ILogger<ExampleController> _logger;

        public ExampleController(IGiftBLL giftService, ILogger<ExampleController> logger)
        {
            _giftService = giftService;
            _logger = logger;
        }

        /// <summary>
        /// דוגמה 1: זריקת BusinessException
        /// סטטוס קוד: 409 Conflict
        /// </summary>
        [HttpPost("transfer-invalid")]
        public async Task<IActionResult> TransferInvalidGift(int giftId)
        {
            _logger.LogInformation("ניסיון להעברת מתנה לא תקינה");

            // זה יזרוק BusinessException שיתופסה על ידי ExceptionHandlingMiddleware
            throw new BusinessException("כמות המתנה חייבת להיות גדולה מ-0");
        }

        /// <summary>
        /// דוגמה 2: זריקת KeyNotFoundException
        /// סטטוס קוד: 404 Not Found
        /// </summary>
        [HttpGet("get-nonexistent/{id}")]
        public async Task<IActionResult> GetNonexistentGift(int id)
        {
            _logger.LogInformation("חיפוש מתנה שאינה קיימת: {GiftId}", id);

            // זה יזרוק KeyNotFoundException שיתופסה על ידי ExceptionHandlingMiddleware
            throw new KeyNotFoundException($"המתנה עם ID {id} לא קיימת");
        }

        /// <summary>
        /// דוגמה 3: זריקת ArgumentException
        /// סטטוס קוד: 400 Bad Request
        /// </summary>
        [HttpPost("create-invalid")]
        public async Task<IActionResult> CreateInvalidGift(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger.LogWarning("ניסיון ליצור מתנה ללא שם");

                // זה יזרוק ArgumentException שיתופסה על ידי ExceptionHandlingMiddleware
                throw new ArgumentException("שם המתנה לא יכול להיות ריק");
            }

            return Ok(new { message = "מתנה נוצרה בהצלחה" });
        }

        /// <summary>
        /// דוגמה 4: שגיאה כללית
        /// סטטוס קוד: 500 Internal Server Error
        /// </summary>
        [HttpGet("trigger-error")]
        public async Task<IActionResult> TriggerError()
        {
            _logger.LogError("זריקת שגיאה כללית לבדיקה");

            // זה יזרוק Exception כללית שיתופסה על ידי ExceptionHandlingMiddleware
            throw new Exception("שגיאה כללית בשרת");
        }

        /// <summary>
        /// דוגמה 5: הצלחה - כל הלוגים יתרשמו
        /// סטטוס קוד: 200 OK
        /// </summary>
        [HttpGet("success/{id}")]
        public async Task<IActionResult> SuccessExample(int id)
        {
            _logger.LogInformation("הבקשה עברה בהצלחה");

            return Ok(new 
            { 
                id = id, 
                message = "הבקשה בוצעה בהצלחה",
                timestamp = DateTime.UtcNow
            });
        }
    }
}

/*
דוגמאות של תגובות שתתקבלנה:

1. POST /api/example/transfer-invalid
   Response Status: 409
   Body:
   {
     "statusCode": 409,
     "message": "כמות המתנה חייבת להיות גדולה מ-0",
     "type": "BusinessException"
   }

2. GET /api/example/get-nonexistent/999
   Response Status: 404
   Body:
   {
     "statusCode": 404,
     "message": "המתנה עם ID 999 לא קיימת",
     "type": "KeyNotFoundException"
   }

3. POST /api/example/create-invalid?name=
   Response Status: 400
   Body:
   {
     "statusCode": 400,
     "message": "שם המתנה לא יכול להיות ריק",
     "type": "ArgumentException"
   }

4. GET /api/example/trigger-error
   Response Status: 500
   Body:
   {
     "statusCode": 500,
     "message": "אירעה שגיאה בשרת. אנא נסו שנית מאוחר יותר",
     "type": "InternalServerError"
   }

5. GET /api/example/success/123
   Response Status: 200
   Body:
   {
     "id": 123,
     "message": "הבקשה בוצעה בהצלחה",
     "timestamp": "2026-02-09T10:30:45.123Z"
   }

Logs שיתרשמו:
[Information] בקשה נכנסת - שיטה: GET, נתיב: /api/example/success/123, IP: 127.0.0.1
[Information] הבקשה עברה בהצלחה
[Information] תגובה יוצאת - סטטוס: 200, נתיב: /api/example/success/123, זמן: 45ms
*/
