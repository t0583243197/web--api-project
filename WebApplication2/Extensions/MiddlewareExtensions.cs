using Microsoft.AspNetCore.Builder;
using WebApplication2.Middlewares;

namespace WebApplication2.Extensions
{
    /// <summary>
    /// Extension Methods לרישום Middlewares בצורה נקייה ופשוטה
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// רישום Middleware לטיפול בשגיאות ולוגים
        /// </summary>
        /// <param name="app">IApplicationBuilder - בנאי היישום</param>
        /// <returns>IApplicationBuilder - לשרשור קריאות</returns>
        public static IApplicationBuilder UseCustomExceptionHandling(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }

        /// <summary>
        /// רישום Middleware לרישום בקשות ותגובות
        /// </summary>
        /// <param name="app">IApplicationBuilder - בנאי היישום</param>
        /// <returns>IApplicationBuilder - לשרשור קריאות</returns>
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}
