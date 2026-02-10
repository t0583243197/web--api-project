# Integration Notes - Middlewares Integration

## סיכום ההשתנויות שבוצעו

### 1. קבצים חדשים שנוצרו:

```
WebApplication2/
├── Middlewares/
│   ├── ExceptionHandlingMiddleware.cs       ✅ טיפול בשגיאות גלובלי
│   └── RequestResponseLoggingMiddleware.cs  ✅ רישום בקשות ותגובות
├── Extensions/
│   └── MiddlewareExtensions.cs              ✅ Extension Methods נקיים
└── Controllers/
    └── ExampleController.cs                 ✅ דוגמאות שימוש
```

### 2. קבצים שנערכו:

- **Program.cs**
  - הוספת `using WebApplication2.Extensions;`
  - החלפת ה-Middleware הישן עם המידלווארים החדשים המתקדמים

## מבנה ה-DI וה-Middleware

```
Request Pipeline:
│
├─→ app.UseCustomExceptionHandling()        [1] תופס שגיאות
├─→ app.UseRequestResponseLogging()         [2] רישום בקשות
├─→ app.UseSwagger()
├─→ app.UseHttpsRedirection()
├─→ app.UseStaticFiles()
├─→ app.UseRouting()
├─→ app.UseCors()
├─→ app.UseAuthentication()
├─→ app.UseAuthorization()
├─→ app.MapRazorPages()
└─→ app.MapControllers()
```

## איך זה עובד כשהכל מחובר?

```csharp
1. בקשה נכנסת
   ↓
2. ExceptionHandlingMiddleware ניגש להזדמנות לתפוס שגיאות
   ↓
3. RequestResponseLoggingMiddleware רושם את פרטי הבקשה
   ↓
4. השרשרת ממשיכה (Routing, Auth, Controllers, וכו')
   ↓
5. אם בקונטרולר זורקים Exception - ExceptionHandlingMiddleware תופס אותו
   ↓
6. RequestResponseLoggingMiddleware רושם את התגובה
   ↓
7. התגובה חוזרת לקליינט
```

## דוגמת זרימה עם ShoppingCart API

### תרחיש: POST /api/gift/create עם כשל

```
┌─────────────────────────────────────────┐
│ בקשה: POST /api/gift/create             │
│ Body: { "name": "", "value": 100 }      │
└──────────────────┬──────────────────────┘
                   │
                   ↓
┌─────────────────────────────────────────┐
│ [Logging] בקשה נכנסת                    │
│ - Method: POST                          │
│ - Path: /api/gift/create                │
│ - Body: {"name":"","value":100}         │
└──────────────────┬──────────────────────┘
                   │
                   ↓
┌─────────────────────────────────────────┐
│ [GiftController] CreateGift()           │
│ בדיקה: name is empty                    │
│ זריקה: ArgumentException                │
│        "שם המתנה חייב להיות מלא"        │
└──────────────────┬──────────────────────┘
                   │
                   ↓
┌─────────────────────────────────────────┐
│ [ExceptionHandling] תפיסת Exception     │
│ - סוג: ArgumentException                │
│ - Status Code: 400                      │
│ [Logging] Warning: שגיאה בטיעון         │
└──────────────────┬──────────────────────┘
                   │
                   ↓
┌─────────────────────────────────────────┐
│ [Response] תגובה יוצאת                  │
│ HTTP 400 Bad Request                    │
│ {                                       │
│   "statusCode": 400,                    │
│   "message": "שם המתנה חייב להיות מלא", │
│   "type": "ArgumentException"           │
│ }                                       │
└──────────────────┬──────────────────────┘
                   │
                   ↓
┌─────────────────────────────────────────┐
│ [Logging] תגובה יוצאת                   │
│ - Status Code: 400                      │
│ - Path: /api/gift/create                │
│ - Time: 25ms                            │
└─────────────────────────────────────────┘
```

## ה-DI Container - שירותים שמשמשים את ה-Middlewares

```csharp
// הערה: ILogger מוזרקת באופן אוטומטי לכל Middleware
// כי היא רשומה ב-DI Container כברירת מחדל

// בעוד שירותים מחייבים רישום מפורש:
builder.Services.AddScoped<IGiftBLL, GiftServiceBLL>();
builder.Services.AddDbContext<StoreContext>(...);

// ILogger נוצרת באופן אוטומטי על ידי ASP.NET Core
// עבור כל class מסוג T:
private readonly ILogger<ExceptionHandlingMiddleware> _logger;
```

## עצות להרחבה עתידית

### 1. הוספת Rate Limiting Middleware

```csharp
// Middlewares/RateLimitingMiddleware.cs
public class RateLimitingMiddleware
{
    private readonly Dictionary<string, (int count, DateTime resetTime)> _requests = new();
    
    public async Task InvokeAsync(HttpContext context)
    {
        var clientIp = context.Connection.RemoteIpAddress?.ToString();
        // בדוק מספר בקשות לכתובת IP
        // אם חרוג - החזר 429 Too Many Requests
    }
}
```

### 2. הוספת Security Headers Middleware

```csharp
// Middlewares/SecurityHeadersMiddleware.cs
public class SecurityHeadersMiddleware
{
    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Add("X-Frame-Options", "DENY");
        context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
        await _next(context);
    }
}
```

### 3. הוספת Correlation ID Middleware

```csharp
// Middlewares/CorrelationIdMiddleware.cs
public class CorrelationIdMiddleware
{
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = Guid.NewGuid().ToString();
        context.Items["CorrelationId"] = correlationId;
        context.Response.Headers.Add("X-Correlation-Id", correlationId);
        await _next(context);
    }
}
```

## Testing - בדיקות של ה-Middlewares

### Unit Test Example:

```csharp
[TestMethod]
public async Task ExceptionHandlingMiddleware_Businesception_Returns409()
{
    // Arrange
    var context = new DefaultHttpContext();
    var middleware = new ExceptionHandlingMiddleware(
        next: async (ctx) => throw new BusinessException("Test error"),
        logger: MockLogger
    );

    // Act
    await middleware.InvokeAsync(context);

    // Assert
    Assert.AreEqual(409, context.Response.StatusCode);
}
```

## Logging Configuration

### appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "System": "Warning",
      "WebApplication2.Middlewares": "Debug"
    },
    "Console": {
      "IncludeScopes": true,
      "TimestampFormat": "yyyy-MM-dd HH:mm:ss"
    }
  }
}
```

## Performance Considerations

✅ **מה טוב:**
- Middlewares קלילים ולא משפיעים על Performance
- RequestResponseLogging מוחק את ה-Body בקוד - חסכון בזיכרון
- Exception Handling מעצור את השגיאה במוקדם - מונע crash

⚠️ **אזהרות:**
- רישום Body אם הוא גדול מ-500 תווים עלול להאט את היישום
- Middleware מסדר גבוה בשרשרת משפיע על כל בקשה

## Troubleshooting

### בעיה: Body חסומה לקריאה בשני פעמים

**פתרון:** `Request.EnableBuffering()` מאפשר קריאה חוזרת

### בעיה: Response Stream לא ניתן לשינוי

**פתרון:** לשמור את ה-Body המקורי וכתוב להעתק זמני

### בעיה: Logging מאט את היישום

**פתרון:** הגבל את LogLevel או בחר רק בקשות חשובות ללוג

---

**יוצר:** Middleware Integration System  
**תאריך:** February 9, 2026  
**סטטוס:** Production Ready ✅
