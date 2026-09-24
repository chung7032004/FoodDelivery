
using FoodDelivery.Common;
using FoodDelivery.Middlewares;
using FoodDelivery.Repositories;
using FoodDelivery.Repositories.Implementations;
using FoodDelivery.Repositories.Implements;
using FoodDelivery.Repositories.Interfaces;
using FoodDelivery.Service.Implementations;
using FoodDelivery.Service.Implements;
using FoodDelivery.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);


    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "FoodDelivery API",
            Version = "v1"
        });

        c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Nhập: Bearer {your JWT token}"
        });

        c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

//Cấu hình kết nối Database Postgresql
var configuration = builder.Configuration;
builder.Services.AddDbContext<FoodContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("Default"))
);

//Định nghĩa chính sách CORS
builder.Services.AddCors(options => {
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:5173") // React dev ports
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IAddressRepository,AddressRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IRestaurantRepository,RestaurantRepository>();
builder.Services.AddScoped<IReviewRepository,ReviewRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IPasswordResetOtpRepository,PasswordResetOtpRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IFileService,FileService>();
builder.Services.AddScoped<IAddressService,AddressService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService,CartService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IReviewService,ReviewService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IRestaurantService,RestaurantService>();
builder.Services.AddScoped<IShipperService, ShipperService>();
builder.Services.AddScoped<IShipperRepository, ShipperRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IEmailService,EmailService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();


builder.Services.AddMemoryCache();
// Cấu hình Redis Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration["Redis:ConnectionString"];
});

// Đăng ký các dịch vụ liên quan đến Authorization
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>(); // Đăng ký PermissionHandler để xử lý các yêu cầu về quyền
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>(); // Đăng ký PermissionPolicyProvider để tạo chính sách động dựa trên tên quyền
builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>(); // Đăng ký CustomAuthorizationMiddlewareResultHandler để trả về lỗi 403 chuẩn hóa khi bị từ chối truy cập

builder.Services.AddExceptionHandler<GlobalExceptionHandler>(); // Đăng ký GlobalExceptionHandler để xử lý lỗi toàn cục
builder.Services.AddProblemDetails(); // Đăng ký ProblemDetails để trả về lỗi chuẩn hóa
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.HandleResponse();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            var result = Result.Failure("UNAUTHORIZED", "Yêu cầu xác thực.");
            var jsonOptions = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
            var jsonString = JsonSerializer.Serialize(result,jsonOptions);
            await context.Response.WriteAsync(jsonString);  
        }
    };
        option.TokenValidationParameters = new TokenValidationParameters
        {
            // 1. Xác minh Khóa Bí mật (Khóa quan trọng nhất!)
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenSecretKey"] ?? "default_secret_key_2025_food_delivery")),

            // 2. Xác minh Issuer và Audience
            ValidateIssuer = true,
            ValidIssuer = "foodApi",

            ValidateAudience = true,
            ValidAudience = "foodApi",
            
            // 3. Xác minh Thời gian sống (hết hạn)
            ValidateLifetime = true,
        };
    });


    var app = builder.Build();

    // Cho phép Swagger chạy ở TẤT CẢ môi trường (kể cả Production)
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FoodDelivery API v1");
        c.RoutePrefix = "swagger"; // Hoặc c.RoutePrefix = string.Empty; nếu muốn vừa truy cập URL chính là mở ngay Swagger
    });

    app.UseExceptionHandler(); // Sử dụng middleware xử lý lỗi toàn cục
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    //Kích hoạt CORS (Phải đặt trước UseAuthorization)
    app.UseCors("AllowReactApp");
    app.UseAuthentication(); //Kiểm tra bạn là ai 
    app.UseAuthorization();// Kiểm tra bạn có quyền truy cập gì
    app.UseMiddleware<RestaurantOpenMiddleware>();// chặn order khi quán đóng 
    app.MapControllers();// Kết nối các đường dẫn (Route) tới Controller

    app.Run();

