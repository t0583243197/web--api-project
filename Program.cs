using Microsoft.EntityFrameworkCore;
using WebApplication2.BLL;
using WebApplication2.DAL;
using WebApplication2.Mappings;

var builder = WebApplication.CreateBuilder(args);

// register DbContext (configure your connection string in appsettings.json)
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(GiftMappingProfile));

// register DAL and BLL for DI
builder.Services.AddScoped<IGiftDal, GiftDAL>();
builder.Services.AddScoped<IGiftBLL, GiftServiceBLL>();

builder.Services.AddControllers();
var app = builder.Build();
app.MapControllers();
app.Run();