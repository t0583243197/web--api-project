# ASP.NET Core Middlewares - Exception Handling & Logging

## 📋 סיכום

נוצרו שני Middlewares מתקדמים לטיפול גלובלי בשגיאות ורישום לוגים, שחוברו למערכת ה-DI הקיימת:

### ✨ תכונות ראשיות:

1. **ExceptionHandlingMiddleware** 🛡️
   - תפיסה וטיפול אוטומטי בשגיאות
   - מיפוי סוגי Exception לסטטוס קודים HTTP מתאימים
   - רישום מפורט של כל שגיאה
   - חזרת JSON עם מידע שגיאה נקי

2. **RequestResponseLoggingMiddleware** 📝
   - רישום מפורט של בקשות נכנסות (Method, Path, Body, IP)
   - רישום תגובות יוצאות (Status Code, Body, זמן ביצוע)
   - קיצור Body גדול (>500 תווים)
   - תמיכה בכל שיטות HTTP

3. **MiddlewareExtensions** 🔌
   - Extension Methods נקיים וקלים לשימוש
   - עקרון Single Responsibility
   - קל להרחיב והוסיף Middlewares נוספים

---

## 📁 מבנה הקבצים החדשים

```
WebApplication2/
├── Middlewares/
│   ├── ExceptionHandlingMiddleware.cs        # טיפול בשגיאות
│   └── RequestResponseLoggingMiddleware.cs   # רישום בקשות
├── Extensions/
│   └── MiddlewareExtensions.cs               # Extension Methods
├── Controllers/
│   └── ExampleController.cs                  # דוגמאות שימוש
└── Program.cs                                # מעודכן עם ה-Middlewares
```

---

## 🚀 איך זה עובד?

### סדר ביצוע:

```
בקשה נכנסת
    ↓
ExceptionHandlingMiddleware (try-catch)
    ↓
RequestResponseLoggingMiddleware (שמור בקשה)
    ↓
שרשרת Middlewares אחרים (Auth, Routing, וכו')
    ↓
Controller/Razor Pages
    ↓
אם Exception → ExceptionHandlingMiddleware תופס אותה
    ↓
RequestResponseLoggingMiddleware שמור תגובה
    ↓
תגובה חוזרת לקליינט
```

---

## 💻 דוגמאות שימוש

### דוגמה 1: BusinessException (409)

```csharp
[HttpPost("transfer-gift")]
public async Task<IActionResult> TransferGift(int giftId)
{
    if (gift.Quantity <= 0)
    {
        throw new BusinessException("כמות המתנה אינה חוקית");
        // Response: 409 Conflict
        // Body: {"statusCode":409,"message":"כמות המתנה אינה חוקית","type":"BusinessException"}
    }
    return Ok(gift);
}
```

### דוגמה 2: KeyNotFoundException (404)

```csharp
[HttpGet("{id}")]
public async Task<IActionResult> GetGift(int id)
{
    var gift = await _giftService.GetAsync(id);
    if (gift == null)
    {
        throw new KeyNotFoundException($"המתנה {id} לא קיימת");
        // Response: 404 Not Found
        // Body: {"statusCode":404,"message":"המתנה 1 לא קיימת","type":"KeyNotFoundException"}
    }
    return Ok(gift);
}
```

### דוגמה 3: ArgumentException (400)

```csharp
[HttpPost]
public async Task<IActionResult> CreateGift(CreateGiftDto dto)
{
    if (string.IsNullOrEmpty(dto.Name))
    {
        throw new ArgumentException("שם המתנה חייב להיות מלא");
        // Response: 400 Bad Request
        // Body: {"statusCode":400,"message":"שם המתנה חייב להיות מלא","type":"ArgumentException"}
    }
    return Ok(await _giftService.CreateAsync(dto));
}
```

---

## 📊 דוגמאות לוגים

### בקשה מוצלחת:
```
[Information] בקשה נכנסת - שיטה: GET, נתיב: /api/gift/1, IP: 127.0.0.1
[Information] תגובה יוצאת - סטטוס: 200, נתיב: /api/gift/1, זמן: 45ms
```

### בקשה עם שגיאה:
```
[Information] בקשה נכנסת - שיטה: POST, נתיב: /api/gift, IP: 127.0.0.1, Body: {"name":"","value":100}
[Warning] שגיאה בטיעון: שם המתנה חייב להיות מלא
[Information] תגובה יוצאת - סטטוס: 400, נתיב: /api/gift, זמן: 12ms
```

---

## 🔧 הגדרות Logging

### appsettings.json (Production)
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft": "Error",
      "WebApplication2": "Information"
    }
  }
}
```

### appsettings.Development.json (Development)
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft": "Information",
      "WebApplication2": "Debug"
    }
  }
}
```

---

## 📈 סוגי Status Codes

| Exception | Code | משמעות |
|-----------|------|---------|
| `BusinessException` | 409 | Conflict - הפרה של חוקים עסקיים |
| `ArgumentException` | 400 | Bad Request - טיעון לא תקין |
| `UnauthorizedAccessException` | 401 | Unauthorized - אין הרשאה |
| `KeyNotFoundException` | 404 | Not Found - משאב לא קיים |
| `Exception` (כללית) | 500 | Internal Server Error - שגיאה בשרת |

---

## ✅ בדיקה שהכל עובד

1. **הרץ את היישום:**
   ```powershell
   dotnet run
   ```

2. **שלח בקשה למנוחה:**
   ```bash
   curl -X POST http://localhost:5000/api/example/transfer-invalid
   ```

3. **בדוק את התגובה:**
   ```json
   {
     "statusCode": 409,
     "message": "כמות המתנה חייבת להיות גדולה מ-0",
     "type": "BusinessException"
   }
   ```

4. **בדוק את הלוגים:**
   ```
   דע כמה שורות בפלט האם יש לוגים של בקשה ותגובה
   ```

---

## 🎯 Best Practices

### ✅ עשו:
- זרקו `BusinessException` לשגיאות עסקיות
- זרקו `KeyNotFoundException` כשמשאב לא קיים
- זרקו `ArgumentException` לטיעונים שגויים
- השתמשו ב-`_logger.LogWarning()` לאירועים חשובים
- בדוקו את הלוגים בדיבאג

### ❌ אל תעשו:
- אל תזרקו `Exception` כללית
- אל תתפסו שגיאות בלי להתמודד איתן
- אל תחשפו פרטים של מסד נתונים
- אל תזרקו את אותה שגיאה שוב מבלי להוסיף ערך

---

## 🔌 אינטגרציה עם ה-DI

### כל משתמש `ILogger<T>` יקבל:
```csharp
public class MyService
{
    private readonly ILogger<MyService> _logger;
    
    public MyService(ILogger<MyService> logger)
    {
        _logger = logger;  // מוזרק אוטומטי מ-DI
    }
}
```

### Extension Methods הם חלק מ-ASP.NET Core:
```csharp
app.UseCustomExceptionHandling();      // IApplicationBuilder.UseCustomExceptionHandling()
app.UseRequestResponseLogging();       // IApplicationBuilder.UseRequestResponseLogging()
```

---

## 📚 קבצים דופקומנטציה

- [MIDDLEWARE_DOCUMENTATION.md](./MIDDLEWARE_DOCUMENTATION.md) - תיעוד מלא
- [INTEGRATION_NOTES.md](./INTEGRATION_NOTES.md) - הערות אינטגרציה
- [ExampleController.cs](./WebApplication2/Controllers/ExampleController.cs) - דוגמאות קוד

---

## 🚨 Troubleshooting

### בעיה: Middleware לא תופס שגיאות
**סיבה:** סדר המידלווארים חשוב
**פתרון:** וודא ש-`UseCustomExceptionHandling()` בא לפני המידלווארים האחרים

### בעיה: Body ריק בלוגים
**סיבה:** Body קרוא רק פעם אחת
**פתרון:** `Request.EnableBuffering()` מופעל בקוד

### בעיה: Logging עם מדי הרבה מידע
**סיבה:** LogLevel מגבוה מדי
**פתרון:** שנה את `appsettings.json`

---

## 📞 תמיכה

לשאלות או בעיות:
1. בדוק את הלוגים בקונסולה
2. קרא את MIDDLEWARE_DOCUMENTATION.md
3. בדוק את ExampleController.cs לדוגמאות

---

**יוצר:** Exception Handling & Logging System  
**תאריך יצירה:** February 9, 2026  
**סטטוס:** ✅ Production Ready
