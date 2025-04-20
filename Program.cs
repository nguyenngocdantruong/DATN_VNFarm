using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using VNFarm.Infrastructure.Data;
using VNFarm.Infrastructure.Persistence.Context;
using VNFarm.Infrastructure.Repositories;
using VNFarm.Infrastructure.Services;
using VNFarm_FinalFinal.Interfaces.Repositories;
using VNFarm_FinalFinal.Interfaces.Services;
using VNFarm_FinalFinal.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VNFarm_FinalFinal.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();


// Thêm MVC
builder.Services.AddControllersWithViews();

// Add CORS policy to allow all origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://127.0.0.1:5500", "http://localhost:5500")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// Cấu hình Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.UseInlineDefinitionsForEnums(); 
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "VNFarm API",
        Version = "v1",
        Description = "API cho ứng dụng VNFarm",
        Contact = new OpenApiContact
        {
            Name = "VNFarm Team",
            Email = "support@vnfarm.com"
        }
    });
});

// Add DbContext với In-Memory Database
builder.Services.AddDbContext<VNFarmContext>(options =>
    options.UseInMemoryDatabase("VNFarmDb"));

// Thêm logging configuration
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();  // Log ra console 📝
    logging.AddDebug();    // Log cho debugging 🐛
});

// Thêm cấu hình Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Đăng ký repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IChatRoomRepository, ChatRoomRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IBusinessRegistrationRepository, BusinessRegistrationRepository>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

// Đăng ký các service
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IChatRoomService, ChatRoomService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IBusinessRegistrationService, BusinessRegistrationService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "VNFarm API v1");
        c.RoutePrefix = "swagger"; // Đặt Swagger UI ở route /swagger
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.Use(async (context, next) =>
{
    var token = context.Request.Cookies["jwt"];
    if (!string.IsNullOrEmpty(token))
    {
        context.Request.Headers.Authorization = $"Bearer {token}";
    }
    await next();
});

// Sử dụng CORS policy đã định nghĩa ở trên
app.UseCors("AllowFrontend"); 

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

// Cấu hình routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

// Cấu hình middleware cho các route
app.Map("/admin", adminApp => {
    adminApp.UseRouting();
    adminApp.UseAuthorization();
    adminApp.UseAdminOnly();
    adminApp.UseEndpoints(endpoints => {
        endpoints.MapControllerRoute(
            name: "admin",
            pattern: "{controller=Admin}/{action=Index}/{id?}");
    });
});

app.Map("/user", userApp => {
    userApp.UseRouting();
    userApp.UseAuthorization();
    userApp.UseUserOnly();
    userApp.UseEndpoints(endpoints => {
        endpoints.MapControllerRoute(
            name: "user",
            pattern: "{controller=User}/{action=Index}/{id?}");
    });
});


// Seed dữ liệu mẫu
await InMemoryDbSeeder.SeedDatabaseAsync(app);

app.Run();
