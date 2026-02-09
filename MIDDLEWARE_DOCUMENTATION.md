# Middleware - Exception Handling ו-Logging

## תיאור כללי

הפרויקט כולל שני Middlewares מתקדמים לטיפול גלובלי בשגיאות ורישום לוגים:

### 1. ExceptionHandlingMiddleware
טיפול מרכזי בכל השגיאות שלא תופסות במקום אחר בקוד.

**תכונות:**
- ✅ ריגיל בקשות נכנסות (Method, Path, IP)
- ✅ טיפול בחריגויות שונות בהתאם לסוגן:
  - `BusinessException` → 409 Conflict
  - `ArgumentException` → 400 Bad Request
  - `UnauthorizedAccessException` → 401 Unauthorized
  - `KeyNotFoundException` → 404 Not Found
  - שגיאות כלליות → 500 Internal Server Error
- ✅ רישום מפורט בעזרת ILogger
- ✅ החזרת JSON עם מידע שגיאה

### 2. RequestResponseLoggingMiddleware
רישום מפורט של כל בקשה ותגובה.

**תכונות:**
- ✅ רישום פרטי הבקשה (Method, Path, Query, Body, IP)
- ✅ רישום תגובה (Status Code, Body, זמן ביצוע)
- ✅ תמיכה ב-GET/POST/PUT/PATCH/DELETE
- ✅ קיצור Body אם הוא ארוך מ-500 תווים
- ✅ מדידת זמן ביצוע הבקשה

## איך זה עובד?

### סדר ה-Middlewares ב-Program.cs

```csharp
// הוספת Middlewares לפני Authentication/Authorization
app.UseCustomExceptionHandling();       // טיפול בשגיאות
app.UseRequestResponseLogging();        // רישום בקשות ותגובות

// ואז המידלווארים האחרים...
app.UseSwagger();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
```

**חשוב:** סדר המידלווארים משנה!

## דוגמאות שימוש

### דוגמה 1: זריקת BusinessException

```csharp
public async Task<IActionResult> TransferGift(int giftId, int donorId)
{
    var gift = await _giftService.GetGiftAsync(giftId);
    
    if (gift == null)
    {
        // הזריקה תופסה על ידי ExceptionHandlingMiddleware
        // תחזור 404 עם JSON
        throw new KeyNotFoundException("המתנה לא קיימת");
    }
    
    if (gift.Quantity <= 0)
    {
        // הזריקה תופסה על ידי ExceptionHandlingMiddleware
        // תחזור 409 עם JSON
        throw new BusinessException("כמות המתנה אינה חוקית");
    }
    
    return Ok(await _giftService.TransferGiftAsync(giftId, donorId));
}
```

### דוגמה 2: צפיית לוגים

כל בקשה תיתרשם באופן הבא:

```
[Information] בקשה נכנסת - שיטה: POST, נתיב: /api/gift, Query: , IP: 127.0.0.1, Body: {"name":"חתונה","value":100}
[Information] תגובה יוצאת - סטטוס: 200, נתיב: /api/gift, זמן: 125ms, Body: {"id":1,"name":"חתונה","value":100}
```

### דוגמה 3: שגיאה בבקשה

```
[Error] שגיאה לא טיפולה התרחשה - שיטה: POST, נתיב: /api/gift
[Warning] שגיאה בטיעון: Gift name cannot be null
```

ה-Response שיתקבל:
```json
{
  "statusCode": 400,
  "message": "Gift name cannot be null",
  "type": "ArgumentException"
}
```

## הוספת ה-Middlewares לקבצים

### הקבצים החדשים שנוצרו:

1. **Middlewares/ExceptionHandlingMiddleware.cs** - טיפול בשגיאות
2. **Middlewares/RequestResponseLoggingMiddleware.cs** - רישום בקשות
3. **Extensions/MiddlewareExtensions.cs** - Extension Methods

### התאמה ב-Program.cs:

1. הוספת `using WebApplication2.Extensions;`
2. הוספת קריאה ל:
   ```csharp
   app.UseCustomExceptionHandling();
   app.UseRequestResponseLogging();
   ```

## קונפיגורציית Logging

### appsettings.json - הגדרות לוגים

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "WebApplication2": "Information"
    }
  }
}
```

### appsettings.Development.json - הגדרות לפיתוח

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

## סוגי Exceptions שנתמכים

| Exception | Status Code | משמעות |
|-----------|-------------|---------|
| BusinessException | 409 | שגיאה עסקית - הפרה של חוקים |
| ArgumentException | 400 | טיעון לא תקין |
| UnauthorizedAccessException | 401 | אין הרשאה |
| KeyNotFoundException | 404 | משאב לא נמצא |
| Exception (כללית) | 500 | שגיאה בשרת |

## עצות לשימוש טוב

✅ **עשו:**
- זרקו `BusinessException` לשגיאות עסקיות
- זרקו `KeyNotFoundException` למשאבים שלא קיימים
- זרקו `ArgumentException` לטיעונים לא תקינים
- השתמשו ב-`_logger.LogWarning()` לאירועים חשובים

❌ **אל תעשו:**
- אל תזרקו `Exception` הכללית - השתמשו בסוגים ספציפיים
- אל תתפסו שגיאות בלי לעשות משהו איתן
- אל תחשפו שגיאות פנימיות של מסד הנתונים ללקוח

## בעיות אפשריות וצורת פתרון

**בעיה:** ה-Middleware לא תופס שגיאות מסוגי controllers

**פתרון:** וודא שה-Middleware הוסף לפני `app.MapRazorPages()` ו-`app.MapControllers()`

**בעיה:** Body הבקשה קטע או ריק

**פתרון:** זה יכול להתרחש כשתוכן הבקשה גדול. השמט את Body מה-logging או הגבל את גודלו.

**בעיה:** Logging קטן מדי או גדול מדי

**פתרון:** שנה את `LogLevel` ב-appsettings.json לפי הצורך.
