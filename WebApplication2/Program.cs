using Microsoft.EntityFrameworkCore; // מייבא EF Core
using WebApplication2.BLL; // מייבא BLL
using WebApplication2.DAL; // מייבא DAL
using Microsoft.AspNetCore.Authentication.JwtBearer; // מייבא הגדרות JWT
using Microsoft.IdentityModel.Tokens; // מייבא סוגי טוקן
using Microsoft.OpenApi.Models; // מייבא סוגי OpenAPI
using System.Text; // מייבא Encoding
using AutoMapper; // מייבא AutoMapper

var builder = WebApplication.CreateBuilder(args); // יצירת WebApplicationBuilder

var key = Encoding.ASCII.GetBytes("YourSuperSecretKeyHere1234567890!"); // מפתח סימטרי לדוגמה

builder.Services.AddAuthentication(options => // רישום Authentication
{ // התחלת קונפיגורציה
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // הגדרת scheme ברירת מחדל
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // הגדרת challenge scheme
}) // סיום AddAuthentication
.AddJwtBearer(options => // הוספת JwtBearer
{ // התחלת קונפיגורציה JwtBearer
    options.TokenValidationParameters = new TokenValidationParameters // פרמטרים לאימות הטוקן
    { // התחלת פרמטרים
        ValidateIssuer = false, // בדיקת issuer (כבוי בדוגמה)
        ValidateAudience = false, // בדיקת audience (כבוי בדוגמה)
        ValidateLifetime = true, // בדיקת תוקף
        ValidateIssuerSigningKey = true, // בדיקת החתימה
        IssuerSigningKey = new SymmetricSecurityKey(key) // מפתח החתימה
    }; // סיום פרמטרים
}); // סיום AddJwtBearer

builder.Services.AddEndpointsApiExplorer(); // רישום API explorer
builder.Services.AddSwaggerGen(c => // רישום Swagger
{ // התחלת קונפיגורציה Swagger
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Auction API", Version = "v1" }); // הגדרת מסמך Swagger

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme // הוספת הגדרת אבטחה
    { // התחלת הגדרת אבטחה
        Description = "אנא הכנס את ה-Token בפורמט הזה: Bearer {your_token}", // תיאור לשדה Authorization
        Name = "Authorization", // שם הכותרת
        In = ParameterLocation.Header, // מיקום הפרמטר
        Type = SecuritySchemeType.ApiKey, // סוג סכימה
        Scheme = "Bearer" // סכימת Bearer
    });

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

builder.Services.AddAutoMapper(typeof(Program).Assembly); // רישום AutoMapper
builder.Services.AddDbContext<StoreContext>(options => // רישום DbContext
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // חיבור למסד SQL


// רישום שירותים DAL/BLL
builder.Services.AddScoped<IGiftDal, GiftDAL>(); // רישום GiftDAL
builder.Services.AddScoped<IGiftBLL, GiftServiceBLL>(); // רישום Gift BLL
builder.Services.AddScoped<IDonorDal, DonorDAL>(); // רישום DonorDAL (אם קיים)
builder.Services.AddScoped<IDonorBLL, DonorServiceBLL>(); // רישום Donor BLL (אם קיים)
builder.Services.AddScoped<ICategoryDal, CategoryDAL>(); // רישום CategoryDAL (אם קיים)
builder.Services.AddScoped<ICategoryBLL, CategoryServiceBLL>(); // רישום Category BLL (אם קיים)
builder.Services.AddScoped<IOrderDal, OrderDAL>(); // רישום OrderDAL (אם קיים)
builder.Services.AddScoped<IOrderBLL, OrderServiceBLL>(); // רישום Order BLL (אם קיים)

// רישום שירותי משתמשים
builder.Services.AddScoped<IUserDal, UserDAL>(provider => // רישום מותאם של UserDAL

{
    var context = provider.GetRequiredService<StoreContext>(); // קבלת StoreContext מ-DI
    var mapper = provider.GetRequiredService<IMapper>(); // קבלת IMapper מ-DI
    return new UserDAL(context, mapper); // יצירת מופע UserDAL
});
builder.Services.AddScoped<IUserBll, UserServiceBLL>(); // רישום User BLL

builder.Services.AddControllers(); // רישום Controllers
builder.Services.AddRazorPages(); // רישום Razor Pages

var app = builder.Build(); // בניית היישום

app.Use(async (context, next) => // Middleware לטיפול גלובלי בשגיאות
{
    try // ניסיון להריץ את ה-Middleware הבא
    {
        await next(); // המשך שרשרת המידלוואר
    }
    catch (Exception ex) // במקרה של שגיאה
    {
        Console.WriteLine($"[ERROR LOG]: {ex.Message}"); // רישום שגיאה בקונסולה
        context.Response.StatusCode = 500; // סטטוס שגיאה
        context.Response.ContentType = "application/json"; // סוג תוכן JSON
        await context.Response.WriteAsJsonAsync(new { error = "אירעה שגיאה בשרת", message = ex.Message }); // החזרת פרטי שגיאה ללקוח
    }
});

if (app.Environment.IsDevelopment()) // במצב פיתוח
{
    app.UseSwagger(); // הפעלת Swagger
    app.UseSwaggerUI(); // הפעלת Swagger UI
}
else // במצב פרודקשן
{
    app.UseExceptionHandler("/Error"); // ניתוב לדף שגיאה
    app.UseHsts(); // הפעלת HSTS
}

app.UseHttpsRedirection(); // ניתוב ל-HTTPS
app.UseStaticFiles(); // הגשת קבצים סטטיים
app.UseRouting(); // הפעלת ניתוב

app.UseAuthentication(); // הפעלת Authentication
app.UseAuthorization(); // הפעלת Authorization

app.MapRazorPages(); // מיפוי Razor Pages
app.MapControllers(); // מיפוי Controllers

app.Run(); // הרצת האפליקציה