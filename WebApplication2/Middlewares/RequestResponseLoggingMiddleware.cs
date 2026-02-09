using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebApplication2.Middlewares
{
    /// <summary>
    /// Middleware לרישום מפורט של בקשות ותגובות (Request/Response Logging)
    /// </summary>
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            // רישום פרטי הבקשה
            await LogRequestAsync(context);

            // שמירת stream המקורי של התגובה
            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                try
                {
                    // עיבוד הבקשה
                    await _next(context);

                    stopwatch.Stop();

                    // רישום פרטי התגובה
                    await LogResponseAsync(context, responseBody, stopwatch.ElapsedMilliseconds);

                    // העברת stream התגובה לחזרה לקליינט
                    await responseBody.CopyToAsync(originalBodyStream);
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    _logger.LogError(ex, "שגיאה במהלך עיבוד הבקשה");
                    throw;
                }
                finally
                {
                    context.Response.Body = originalBodyStream;
                }
            }
        }

        /// <summary>
        /// רישום פרטי הבקשה
        /// </summary>
        private async Task LogRequestAsync(HttpContext context)
        {
            var request = context.Request;
            var body = "";

            // קריאת body הבקשה אם היא POST/PUT/PATCH
            if (request.Method != "GET" && request.Method != "HEAD")
            {
                request.EnableBuffering();
                using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, leaveOpen: true))
                {
                    body = await reader.ReadToEndAsync();
                    request.Body.Position = 0;
                }
            }

            _logger.LogInformation(
                "בקשה נכנסת - שיטה: {Method}, נתיב: {Path}, Query: {Query}, IP: {RemoteIP}, Body: {Body}",
                request.Method,
                request.Path,
                request.QueryString,
                context.Connection.RemoteIpAddress,
                body.Length > 500 ? body[..497] + "..." : body
            );
        }

        /// <summary>
        /// רישום פרטי התגובה
        /// </summary>
        private async Task LogResponseAsync(HttpContext context, MemoryStream responseBody, long elapsedMilliseconds)
        {
            var response = context.Response;
            responseBody.Seek(0, SeekOrigin.Begin);

            string body = await new StreamReader(responseBody).ReadToEndAsync();

            _logger.LogInformation(
                "תגובה יוצאת - סטטוס: {StatusCode}, נתיב: {Path}, זמן: {ElapsedMs}ms, Body: {Body}",
                response.StatusCode,
                context.Request.Path,
                elapsedMilliseconds,
                body.Length > 500 ? body[..497] + "..." : body
            );

            responseBody.Seek(0, SeekOrigin.Begin);
        }
    }
}
