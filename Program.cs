using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using VNFarm.Data.Data;
using VNFarm.Data;
using VNFarm.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VNFarm.Services;
using VNFarm.Entities;
using VNPAY.NET;
using VNFarm.ExternalServices.Payment;
using VNFarm.ExternalServices.Email;
using PusherServer;
using VNFarm.Caching;
using VNFarm.Repositories.Interfaces;
using VNFarm.Services.Interfaces;
using VNFarm.Services.External.Interfaces;
using VNFarm.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// Add memory cache
builder.Services.AddMemoryCache();

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
        // Cho phép SignalR lấy JWT qua query string
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chatHub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddScoped(typeof(MyOtpService));


// Add CORS policy to allow all origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://127.0.0.1:5500", "http://localhost:5500", "http://localhost:5011", "http://127.0.0.1:5011", "http://192.168.1.139:5011")
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
{
    // options.UseInMemoryDatabase("VNFarmDb");
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

// Cấu hình Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) 
    .CreateLogger();

builder.Host.UseSerilog();

// Dont use this anymore
// builder.Services.AddLogging(logging =>
// {
//     logging.ClearProviders();
//     logging.AddConsole(); 
//     logging.AddDebug();
// });

//Đăng ký VNPAY
builder.Services.AddScoped<IVnpay, Vnpay>();

// Đăng ký repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IChatRoomRepository, ChatRoomRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IPaymentService, VnpayPayment>();

// Đăng ký các service
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IChatRoomService, ChatRoomService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddTransient<IEmailService, EmailService>();


// Add Pusher
builder.Services.AddSingleton(new Pusher(
    appId: builder.Configuration["Pusher:AppId"],
    appKey: builder.Configuration["Pusher:Key"],
    appSecret: builder.Configuration["Pusher:Secret"],
    options: new PusherOptions
    {
        Cluster = builder.Configuration["Pusher:Cluster"],
        Encrypted = true
    }));


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

// app.UseHttpsRedirection();
app.MapStaticAssets();
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
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/Error/{0}"); // xử lý 404, 401
app.UseExceptionHandler("/Error/500"); // xử lý lỗi 500

// Cấu hình routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();


// Seed dữ liệu mẫu
// await InMemoryDbSeeder.SeedDatabaseAsync(app);

app.Run();
