using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebApplication2.BLL;

namespace WebApplication2.Middlewares
{
    /// <summary>
    /// Middleware לטיפול גלובלי בשגיאות (Exception Handling) ורישום לוגים
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        /// <summary>
        /// בנאי Middleware
        /// </summary>
        /// <param name="next">בקשה הבאה בשרשרת (request delegate)</param>
        /// <param name="logger">שירות לרישום לוגים</param>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invoke - מתודה עיקרית של ה-Middleware
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // רישום פרטי הבקשה הנכנסת
                LogIncomingRequest(context);

                // המשך לעיבוד הבקשה
                await _next(context);

                // רישום בקשה שהושלמה בהצלחה
                LogOutgoingResponse(context);
            }
            catch (Exception ex)
            {
                // רישום השגיאה והחזרת תגובה מתאימה
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// רישום פרטי הבקשה הנכנסת
        /// </summary>
        private void LogIncomingRequest(HttpContext context)
        {
            var request = context.Request;
            _logger.LogInformation(
                "בקשה נכנסת - שיטה: {Method}, נתיב: {Path}, IP: {RemoteIP}",
                request.Method,
                request.Path,
                context.Connection.RemoteIpAddress
            );
        }

        /// <summary>
        /// רישום תגובה יוצאת
        /// </summary>
        private void LogOutgoingResponse(HttpContext context)
        {
            _logger.LogInformation(
                "תגובה יוצאת - סטטוס: {StatusCode}, נתיב: {Path}",
                context.Response.StatusCode,
                context.Request.Path
            );
        }

        /// <summary>
        /// טיפול בשגיאות ויצירת תגובת שגיאה מתאימה
        /// </summary>
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // רישום השגיאה
            _logger.LogError(exception,
                "שגיאה לא טיפולה התרחשה - שיטה: {Method}, נתיב: {Path}",
                context.Request.Method,
                context.Request.Path
            );

            context.Response.ContentType = "application/json";

            // קביעת סטטוס קוד וביצוע טיפול לפי סוג השגיאה
            if (exception is BusinessException businessException)
            {
                // שגיאות עסקיות (טיעונים לא תקינים, הפרה של חוקים עסקיים)
                context.Response.StatusCode = (int)HttpStatusCode.Conflict; // 409
                _logger.LogWarning("שגיאה עסקית: {Message}", businessException.Message);

                var response = new
                {
                    statusCode = 409,
                    message = businessException.Message,
                    type = "BusinessException"
                };

                return context.Response.WriteAsJsonAsync(response);
            }
            else if (exception is ArgumentException argException)
            {
                // שגיאות טיעון (null/invalid input)
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // 400
                _logger.LogWarning("שגיאה בטיעון: {Message}", argException.Message);

                var response = new
                {
                    statusCode = 400,
                    message = argException.Message,
                    type = "ArgumentException"
                };

                return context.Response.WriteAsJsonAsync(response);
            }
            else if (exception is UnauthorizedAccessException)
            {
                // שגיאות הרשאה
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized; // 401
                _logger.LogWarning("ניסיון גישה לא מורשה");

                var response = new
                {
                    statusCode = 401,
                    message = "אתה לא מורשה לגשת למשאב זה",
                    type = "UnauthorizedAccessException"
                };

                return context.Response.WriteAsJsonAsync(response);
            }
            else if (exception is KeyNotFoundException)
            {
                // משאב לא נמצא
                context.Response.StatusCode = (int)HttpStatusCode.NotFound; // 404
                _logger.LogWarning("משאב לא נמצא: {Message}", exception.Message);

                var response = new
                {
                    statusCode = 404,
                    message = exception.Message,
                    type = "KeyNotFoundException"
                };

                return context.Response.WriteAsJsonAsync(response);
            }
            else
            {
                // שגיאות כלליות שלא טיפלנו בהן - שגיאה פנימית בשרת
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // 500

                var response = new
                {
                    statusCode = 500,
                    message = "אירעה שגיאה בשרת. אנא נסו שנית מאוחר יותר",
                    type = "InternalServerError"
                };

                return context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
