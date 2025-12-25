using Microsoft.EntityFrameworkCore; // מייבא EF Core
using WebApplication2.BLL; // מייבא BLL
using WebApplication2.DAL; // מייבא DAL
using Microsoft.AspNetCore.Authentication.JwtBearer; // מייבא הגדרות JWT
using Microsoft.IdentityModel.Tokens; // מייבא סוגי טוקן
using Microsoft.OpenApi.Models; // מייבא סוגי OpenAPI
using System.Text; // מייבא Encoding
using AutoMapper; // מייבא AutoMapper

// -----------------------------
// Program.cs – תמצית ותפקיד הקובץ
// -----------------------------
// קובץ האתחול של היישום (entry point) ב־ASP.NET Core (Razor Pages).
// - מקים WebApplicationBuilder ומשתמש ב־DI להוספה שירותים (Services).
// - מגדיר אמצעי אימות/אבטחה (Authentication / Authorization).
// - מגדיר Swagger לפיתוח/תיעוד API.
// - בונה שרשרת מידלוואר (Middleware) ומתחיל את היישום באמצעות app.Run().
// הערות שימושיות:
// - שמירת מפתחות/מחרוזות רגישות: אל תניחו מפתח קשיח בקוד — השתמשו ב־appsettings / User Secrets / Key Vault.
// - סדר המידלווארים חשוב: UseAuthentication חייב לבוא לפני UseAuthorization וכו'.
// -----------------------------

var builder = WebApplication.CreateBuilder(args); // יצירת WebApplicationBuilder: אוסף קונפיגורציה, DI ו־logging

// מפתח סימטרי לדוגמה לשימוש בחתימת JWT.
// הערה בטיחותית: זה רק להדגמה — יש לאחסן במיקום מאובטח ולהשתמש בערכים מתצורה ב‑prod.
var key = Encoding.ASCII.GetBytes("YourSuperSecretKeyHere1234567890!");

// -----------------------------
// Authentication (אימות זהות)
// -----------------------------
// AddAuthentication מגדיר את ה־schemes הברירת מחדל לאימות (כאן: JwtBearer).
// AddJwtBearer מגדיר כיצד יש לאמת טוקנים שמגיעים ב־Authorization: Bearer {token}.
builder.Services.AddAuthentication(options => // רישום Authentication
{ // התחלת קונפיגורציה
    // הגדרת ה־scheme שישמש כברירת מחדל לאימות ואתגר (challenge)
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // הגדרת scheme ברירת מחדל
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // הגדרת challenge scheme
}) // סיום AddAuthentication
.AddJwtBearer(options => // הוספת JwtBearer
{ // התחלת קונפיגורציה JwtBearer
    // TokenValidationParameters מגדיר אילו בדיקות לבצע על ה‑JWT:
    // - ValidateIssuer: האם לבדוק את ה‑iss (מנפיק הטוקן).
    // - ValidateAudience: האם לבדוק את ה‑aud (היעד של הטוקן).
    // - ValidateLifetime: בדיקת תוקף (exp / nbf).
    // - ValidateIssuerSigningKey: בדיקת החתימה בעזרת המפתח/מפתח ציבורי.
    // - IssuerSigningKey: המפתח המשמש לאימות החתימה (כאן - Symmetric).
    options.TokenValidationParameters = new TokenValidationParameters // פרמטרים לאימות הטוקן
    { // התחלת פרמטרים
        // בדוגמה זו כיבינו בדיקות issuer/audience (לא מומלץ בפרודקשן).
        // רצוי להגדיר ValidateIssuer = true ו־ValidIssuer = configuration["Jwt:Issuer"]
        ValidateIssuer = false, // בדיקת issuer (כבוי בדוגמה)
        ValidateAudience = false, // בדיקת audience (כבוי בדוגמה)

        // תמיד להפעיל ValidateLifetime בשרת כדי למנוע שימוש בטוקנים שפגו.
        ValidateLifetime = true, // בדיקת תוקף

        // יש לאמת שהחתימה תקפה — חובה כאשר משתמשים בחתימה סימטרית/אסימטרית.
        ValidateIssuerSigningKey = true, // בדיקת החתימה
        IssuerSigningKey = new SymmetricSecurityKey(key) // מפתח החתימה
    }; // סיום פרמטרים

    // אופציות נוסxxx שאפשר להוסיף (לא מופיעות כאן בדוגמה):
    // options.RequireHttpsMetadata = true; // למנוע שימוש ב־HTTP בעת פיתוח/פרודקשן
    // options.SaveToken = true;
    // options.Events = new JwtBearerEvents { OnAuthenticationFailed = ..., OnTokenValidated = ... };
}); // סיום AddJwtBearer

// -----------------------------
// Swagger / OpenAPI
// -----------------------------
// Swagger מספק תיעוד אינטראקטיבי של ה־API. כאן מוסיפים תיעוד בסיסי ויכולת להזין טוקן Bearer ב־UI.
builder.Services.AddEndpointsApiExplorer(); // רישום API explorer
builder.Services.AddSwaggerGen(c => // רישום Swagger
{ // התחלת קונפיגורציה Swagger
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Auction API", Version = "v1" }); // הגדרת מסמך Swagger

    // הוספת שדה אבטחה מסוג Bearer ל־Swagger UI:
    // זה מאפשר להזין Authorization: Bearer {token} דרך הממשק של Swagger.
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme // הוספת הגדרת אבטחה
    { // התחלת הגדרת אבטחה
        Description = "אנא הכנס את ה-Token בפורמט הזה: Bearer {your_token}", // תיאור לשדה Authorization
        Name = "Authorization", // שם הכותרת
        In = ParameterLocation.Header, // מיקום הפרמטר
        Type = SecuritySchemeType.ApiKey, // סוג סכימה
        Scheme = "Bearer" // סכימת Bearer
    });

    // מחייב הוספת טוקן בכל הבקשות ב־Swagger UI (ניתן להתאים לפי צורך).
    c.AddSecurityRequirement(new OpenApiSecurityRequirement // הוספת דרישת אבטחה גלובלית
    { // תחילת דרישה
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } // הפניה להגדרת ה-Bearer
            },
            new string[] {} // ללא scopes נוספים
        }
    });
}); // סיום SwaggerGen

// -----------------------------
// שירותים נוספים (DI) והרשאות AutoMapper/DbContext
// -----------------------------
builder.Services.AddAutoMapper(typeof(Program).Assembly); // רישום AutoMapper – מפה בין מודלים/DTOs
// רישום DbContext – חיבור ל‑SQL Server עם מחרוזת החיבור מהקונפיגורציה (appsettings.json / secrets)
builder.Services.AddDbContext<StoreContext>(options => // רישום DbContext
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // חיבור למסד SQL

// -----------------------------
// רישום שירותי DAL/BLL ב־Dependency Injection
// -----------------------------
// AddScoped: חיוניות להרצה בפרוסקופ של בקשה HTTP (Scope per request).
// - אם רוצים singleton/service יחיד לכל היישום יש להשתמש ב־AddSingleton.
// - אם רוצים transient (מופע חדש בכל בקשה ל־constructor) יש AddTransient.
builder.Services.AddScoped<IGiftDal, GiftDAL>(); // רישום GiftDAL
builder.Services.AddScoped<IGiftBLL, GiftServiceBLL>(); // רישום Gift BLL
builder.Services.AddScoped<IDonorDal, DonorDAL>(); // רישום DonorDAL (אם קיים)
builder.Services.AddScoped<IDonorBLL, DonorServiceBLL>(); // רישום Donor BLL (אם קיים)
builder.Services.AddScoped<ICategoryDal, CategoryDAL>(); // רישום CategoryDAL (אם קיים)
builder.Services.AddScoped<ICategoryBLL, CategoryServiceBLL>(); // רישום Category BLL (אם קיים)
builder.Services.AddScoped<IOrderDal, OrderDAL>(); // רישום OrderDAL (אם קיים)
builder.Services.AddScoped<IOrderBLL, OrderServiceBLL>(); // רישום Order BLL (אם קיים)



// דוגמה של רישום מותאם: יצירת UserDAL עם תלויות ידניות מה־DI (context + mapper).
// שימוש ב־factory שימושי כאשר הבנאי של השירות דורש פרמטרים או לוגיקה מיוחדת.
builder.Services.AddScoped<IUserDal, UserDAL>(provider => // רישום מותאם של UserDAL
{
    var context = provider.GetRequiredService<StoreContext>(); // קבלת StoreContext מ‑DI
    var mapper = provider.GetRequiredService<IMapper>(); // קבלת IMapper מ‑DI
    return new UserDAL(context, mapper); // יצירת מופע UserDAL
});
builder.Services.AddScoped<IUserBll, UserServiceBLL>(); // רישום User BLL

// רישום MVC controllers ו־Razor Pages (הפרויקט הוא Razor Pages ולכן חשוב).
builder.Services.AddControllers(); // רישום Controllers (למקרה שיש API controllers)
builder.Services.AddRazorPages(); // רישום Razor Pages (UI הפניה לדפים)

// -----------------------------
// בנייה והרצת היישום
// -----------------------------
var app = builder.Build(); // בניית היישום מה־builder וה־service container

// -----------------------------
// Middleware — טיפול גלובלי בשגיאות (דוגמה פשוטה)
// -----------------------------
// כאן מוסיף Middleware תופס חריגות מטרמינליות ומחזיר StatusCode 500 עם JSON.
// שים לב: ב‑prod עדיף להשתמש ב־UseExceptionHandler כדי שלא לחשוף פרטי שגיאות ללקוח.
app.Use(async (context, next) => // Middleware לטיפול גלובלי בשגיאות
{
    try // ניסיון להריץ את ה‑Middleware הבא בשרשרת
    {
        await next(); // המשך שרשרת המידלוואר
    }
    catch (Exception ex) // במקרה של שגיאה שלא תופסה במקום אחר
    {
        // לוג בסיסי לקונסולה — עדיף להשתמש ב־ILogger במערכת אמיתית.
        Console.WriteLine($"[ERROR LOG]: {ex.Message}"); // רישום שגיאה בקונסולה
        context.Response.ContentType = "application/json";

        if (ex is BusinessException) // map business errors to 409 Conflict (or 400)
        {
            context.Response.StatusCode = 409;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        else
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { error = "אירעה שגיאה בשרת" });
        }
    }
});

// -----------------------------
// טיפול בסביבת הרצה (Development / Production)
// -----------------------------
if (app.Environment.IsDevelopment()) // במצב פיתוח
{
    // פקודות פתוחות לפיתוח בלבד: Swagger UI ועוד כלים לדיבאג.
    app.UseSwagger(); // הפעלת Swagger
    app.UseSwaggerUI(); // הפעלת Swagger UI
}
else // במצב פרודקשן
{
    // במצב פרודקשן נשתמש ב‑ExceptionHandler המתקדם יותר כדי להציג דף שגיאה ידידותי.
    app.UseExceptionHandler("/Error"); // ניתוב לדף שגיאה
    app.UseHsts(); // הפעלת HSTS (אבטחת שכבות רשת, דרוש HTTPS)
}

// -----------------------------
// שימוש במידלווארים סטנדרטיים
// -----------------------------
app.UseHttpsRedirection(); // ניתוב אוטומטי ל־HTTPS (אם זמין)
app.UseStaticFiles(); // הגשת קבצים סטטיים מתיקיית wwwroot
app.UseRouting(); // הפעלת ניתוב — חייב לפני UseAuthentication/UseAuthorization

// סדר חשוב:
// 1. UseAuthentication() – מאמת Identity מהבקשה (מממש את ה־Principal).
// 2. UseAuthorization() – בודק האם ה־Principal מורשה לגשת למשאב.
app.UseAuthentication(); // הפעלת Authentication
app.UseAuthorization(); // הפעלת Authorization

// מיפוי נקודות קצה: Razor Pages ו־Controllers (API)
// חשוב: מיפוי צריך להתבצע אחרי כל המידלווארים שהכרנו למעלה.
app.MapRazorPages(); // מיפוי Razor Pages
app.MapControllers(); // מיפוי Controllers

app.Run(); // הרצת האפליקציה — חסימה עד שנסגר השרת