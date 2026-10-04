using System.Text.RegularExpressions;
using FoodDelivery.Common;
using FoodDelivery.Constants;
using FoodDelivery.DTOs.Restaurant;
using FoodDelivery.Entities;
using FoodDelivery.Repositories;
using FoodDelivery.Repositories.Interfaces;
using FoodDelivery.Service.Interfaces;

namespace FoodDelivery.Service.Implementations;
public class RestaurantService : IRestaurantService
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly FoodContext _context;

    public RestaurantService(IRestaurantRepository restaurantRepository, IUserRepository userRepository, IRoleRepository roleRepository, FoodContext context)
    {
        _restaurantRepository = restaurantRepository;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _context = context;
    }

    // Helper: lấy quán của user đang đăng nhập
    private async Task<Restaurant?> GetRestaurantOfUser(Guid userId)
    {
        var restaurant = await _restaurantRepository.GetRestaurantByUserIdAsync(userId);
        return restaurant?.ApplicationStatus == RestaurantApplicationStatus.Approved ? restaurant : null;
    }

    private bool ComputeIsOperating(Restaurant restaurant)
    {
        var now = DateTime.Now.TimeOfDay;
        if (restaurant.OpenTime <= restaurant.CloseTime)
            return now >= restaurant.OpenTime && now <= restaurant.CloseTime;
        return now >= restaurant.OpenTime || now <= restaurant.CloseTime;
    }

    public async Task<Result<RestaurantProfileDto>> GetMyRestaurantAsync(Guid userId)
    {
        var restaurant = await GetRestaurantOfUser(userId);
        if (restaurant == null)
            return HttpErrors.NotFound<RestaurantProfileDto>("RESTAURANT_NOT_FOUND", "Bạn chưa được gán vào quán nào.");

        var finalStatus = restaurant.IsOpen && ComputeIsOperating(restaurant);
        var response = new RestaurantProfileDto
        {
            Id = restaurant.Id,
            Name = restaurant.Name,
            Address = restaurant.Address,
            Phone = restaurant.Phone,
            OpenTime = restaurant.OpenTime,
            CloseTime = restaurant.CloseTime,
            IsOpen = finalStatus,
            ClosingMessage = restaurant.ClosingMessage,
            Latitude = restaurant.Latitude,
            Longitude = restaurant.Longitude
        };
        return Result<RestaurantProfileDto>.Success(response);
    }

    public async Task<Result<RestaurantStatusResponse>> GetStatusRestaurantAsync(Guid userId)
    {
        var restaurant = await GetRestaurantOfUser(userId);
        if (restaurant == null)
            return HttpErrors.NotFound<RestaurantStatusResponse>("RESTAURANT_NOT_FOUND", "Bạn chưa được gán vào quán nào.");

        var finalStatus = restaurant.IsOpen && ComputeIsOperating(restaurant);
        var response = new RestaurantStatusResponse
        {
            IsOpen = finalStatus,
            ClosingMessage = restaurant.ClosingMessage,
            UpdatedAt = restaurant.UpdatedAt,
        };
        return Result<RestaurantStatusResponse>.Success(response);
    }

    public async Task<Result> OpenRestaurantAsync(Guid userId)
    {
        var restaurant = await GetRestaurantOfUser(userId);
        if (restaurant == null)
            return HttpErrors.NotFound("RESTAURANT_NOT_FOUND", "Bạn chưa được gán vào quán nào.");
        if (restaurant.IsOpen)
            return HttpErrors.BadRequest("RESTAURANT_ALREADY_OPEN", "Quán đang mở.");

        restaurant.IsOpen = true;
        restaurant.ClosingMessage = null;
        restaurant.UpdatedAt = DateTime.UtcNow;
        await _restaurantRepository.UpdateAsync(restaurant);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> CloseRestaurantAsync(Guid userId, CloseRestaurantRequest request)
    {
        var restaurant = await GetRestaurantOfUser(userId);
        if (restaurant == null)
            return HttpErrors.NotFound("RESTAURANT_NOT_FOUND", "Bạn chưa được gán vào quán nào.");
        if (!restaurant.IsOpen)
            return HttpErrors.BadRequest("RESTAURANT_ALREADY_CLOSE", "Quán đang đóng.");

        restaurant.IsOpen = false;
        restaurant.ClosingMessage = request.ClosingMessage;
        restaurant.UpdatedAt = DateTime.UtcNow;
        await _restaurantRepository.UpdateAsync(restaurant);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateRestaurantAsync(Guid userId, UpdateRestaurantProfileRequest request)
    {
        var restaurant = await GetRestaurantOfUser(userId);
        if (restaurant == null)
            return HttpErrors.NotFound("RESTAURANT_NOT_FOUND", "Bạn chưa được gán vào quán nào.");

        restaurant.Name = request.Name;
        restaurant.Address = request.Address;
        restaurant.Phone = request.Phone;
        restaurant.Latitude = request.Latitude;
        restaurant.Longitude = request.Longitude;
        restaurant.OpenTime = request.OpenTime;
        restaurant.CloseTime = request.CloseTime;
        restaurant.UpdatedAt = DateTime.UtcNow;
        await _restaurantRepository.UpdateAsync(restaurant);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> RegisterRestaurantAsync(Guid userId, RegisterRestaurantRequest request)
    {
        if (request == null
            || string.IsNullOrWhiteSpace(request.Name)
            || string.IsNullOrWhiteSpace(request.Address)
            || string.IsNullOrWhiteSpace(request.Phone))
            return HttpErrors.BadRequest("INVALID_INPUT", "Thông tin quán không hợp lệ.");

        if (!Regex.IsMatch(request.Phone, @"^0\d{9}$"))
            return HttpErrors.BadRequest("INVALID_PHONE", "Số điện thoại phải bắt đầu bằng 0 và có đúng 10 chữ số.");

        if (request.OpenTime == request.CloseTime)
            return HttpErrors.BadRequest("INVALID_TIME", "Giờ mở cửa và đóng cửa không được giống nhau.");

        var user = await _userRepository.GetUserByIdWithRoleAsync(userId);
        if(user == null){
            return HttpErrors.NotFound("USER_NOT_FOUND", "Người dùng không tồn tại.");
        }

        var existingRestaurant = await _restaurantRepository.GetRestaurantByUserIdAsync(userId);
        if (existingRestaurant != null && existingRestaurant.ApplicationStatus != RestaurantApplicationStatus.Rejected)
        {
            if (existingRestaurant.ApplicationStatus == RestaurantApplicationStatus.Pending)
            {
                _context.UserRoles.RemoveRange(user.UserRoles.Where(userRole => userRole.RoleId == RoleIds.RestaurantOwner));
                await _context.SaveChangesAsync();
            }
            return HttpErrors.BadRequest("ALREADY_HAS_RESTAURANT", "Bạn đã có hồ sơ đang chờ hoặc đã được duyệt.");
        }

        if (existingRestaurant != null)
        {
            existingRestaurant.Name = request.Name.Trim();
            existingRestaurant.Address = request.Address.Trim();
            existingRestaurant.Phone = request.Phone.Trim();
            existingRestaurant.Latitude = request.Latitude;
            existingRestaurant.Longitude = request.Longitude;
            existingRestaurant.OpenTime = request.OpenTime;
            existingRestaurant.CloseTime = request.CloseTime;
            existingRestaurant.ApplicationStatus = RestaurantApplicationStatus.Pending;
            existingRestaurant.RejectionReason = null;
            existingRestaurant.ReviewedAt = null;
            existingRestaurant.ReviewedByAdminId = null;
            existingRestaurant.IsOpen = false;
            existingRestaurant.UpdatedAt = DateTime.UtcNow;
            _context.UserRoles.RemoveRange(user.UserRoles.Where(userRole => userRole.RoleId == RoleIds.RestaurantOwner));
            await _restaurantRepository.UpdateAsync(existingRestaurant);
            await _context.SaveChangesAsync();
            return Result.Success();
        }
        
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try{
            // Tạo restaurant
            var restaurant = new Restaurant
            {
                OwnerId        = userId,
                Name            = request.Name.Trim(),
                Address         = request.Address.Trim(),
                Phone           = request.Phone.Trim(),
                OpenTime        = request.OpenTime,
                CloseTime       = request.CloseTime,
                Latitude        = request.Latitude, 
                Longitude       = request.Longitude,
                ApplicationStatus = RestaurantApplicationStatus.Pending,
                IsOpen          = false,
                ClosingMessage  = null,
                CreatedAt       = DateTime.UtcNow,
                UpdatedAt       = DateTime.UtcNow
            };
            await _context.Restaurants.AddAsync(restaurant);
            _context.UserRoles.RemoveRange(user.UserRoles.Where(userRole => userRole.RoleId == RoleIds.RestaurantOwner));
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return Result.Success();
        } 
        catch(Exception ex){
            await transaction.RollbackAsync();
            return HttpErrors.InternalServerError("REGISTRATION_FAILED", "Đăng ký quán thất bại: " + ex.Message);
        }
    }

    public async Task<Result<MyRestaurantApplicationResponse>> GetMyApplicationAsync(Guid userId)
    {
        var restaurant = await _restaurantRepository.GetRestaurantByUserIdAsync(userId);
        if (restaurant == null)
            return Result<MyRestaurantApplicationResponse>.Success(new MyRestaurantApplicationResponse { HasApplication = false });

        return Result<MyRestaurantApplicationResponse>.Success(new MyRestaurantApplicationResponse
        {
            HasApplication = true,
            Application = MapApplication(restaurant)
        });
    }

    public async Task<Result<PagedResponse<RestaurantApplicationResponse>>> GetRestaurantsForAdminAsync(int page, int pageSize, RestaurantApplicationStatus? status)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);
        var (items, totalCount) = await _restaurantRepository.GetAllForAdminAsync(page, pageSize, status);
        var data = items.Select(MapApplication).ToList();
        var meta = new PaginationMeta(page, pageSize, totalCount, (int)Math.Ceiling((double)totalCount / pageSize));
        return Result<PagedResponse<RestaurantApplicationResponse>>.Success(new PagedResponse<RestaurantApplicationResponse>(data, meta));
    }

    public async Task<Result> ReviewRestaurantApplicationAsync(Guid restaurantId, Guid adminId, ReviewRestaurantRequest request)
    {
        var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
        if (restaurant == null)
            return HttpErrors.NotFound("RESTAURANT_NOT_FOUND", "Không tìm thấy hồ sơ nhà hàng.");
        if (restaurant.ApplicationStatus != RestaurantApplicationStatus.Pending)
            return HttpErrors.BadRequest("APPLICATION_ALREADY_REVIEWED", "Hồ sơ này đã được xử lý.");
        if (!request.IsApproved && string.IsNullOrWhiteSpace(request.RejectReason))
            return HttpErrors.BadRequest("REJECT_REASON_REQUIRED", "Vui lòng nhập lý do từ chối.");

        if (request.IsApproved)
        {
            var user = await _userRepository.GetUserByIdWithRoleAsync(restaurant.OwnerId);
            if (user == null)
                return HttpErrors.NotFound("USER_NOT_FOUND", "Không tìm thấy chủ nhà hàng.");
            if (!user.UserRoles.Any(userRole => userRole.RoleId == RoleIds.RestaurantOwner))
            {
                var role = await _roleRepository.GetByIdAsync(RoleIds.RestaurantOwner);
                if (role == null)
                    return HttpErrors.NotFound("ROLE_NOT_FOUND", "Không tìm thấy quyền RestaurantOwner.");
                await _userRepository.AddUserRoleAsync(new UserRole { UserId = restaurant.OwnerId, RoleId = role.Id });
            }
            restaurant.ApplicationStatus = RestaurantApplicationStatus.Approved;
            restaurant.RejectionReason = null;
        }
        else
        {
            var user = await _userRepository.GetUserByIdWithRoleAsync(restaurant.OwnerId);
            if (user != null)
                _context.UserRoles.RemoveRange(user.UserRoles.Where(userRole => userRole.RoleId == RoleIds.RestaurantOwner));
            restaurant.ApplicationStatus = RestaurantApplicationStatus.Rejected;
            restaurant.RejectionReason = request.RejectReason!.Trim();
        }

        restaurant.ReviewedAt = DateTime.UtcNow;
        restaurant.ReviewedByAdminId = adminId;
        restaurant.UpdatedAt = DateTime.UtcNow;
        await _restaurantRepository.UpdateAsync(restaurant);
        await _context.SaveChangesAsync();
        return Result.Success();
    }

    private static RestaurantApplicationResponse MapApplication(Restaurant restaurant) => new()
    {
        RestaurantId = restaurant.Id,
        OwnerId = restaurant.OwnerId,
        OwnerName = restaurant.Owner?.FullName ?? string.Empty,
        OwnerEmail = restaurant.Owner?.Email ?? string.Empty,
        OwnerPhone = restaurant.Owner?.Phone ?? string.Empty,
        Name = restaurant.Name,
        Address = restaurant.Address,
        Phone = restaurant.Phone,
        Latitude = restaurant.Latitude,
        Longitude = restaurant.Longitude,
        OpenTime = restaurant.OpenTime,
        CloseTime = restaurant.CloseTime,
        IsOpen = restaurant.IsOpen,
        ApplicationStatus = restaurant.ApplicationStatus,
        RejectionReason = restaurant.RejectionReason,
        CreatedAt = restaurant.CreatedAt,
        ReviewedAt = restaurant.ReviewedAt
    };
}
