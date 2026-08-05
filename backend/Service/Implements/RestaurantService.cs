using FoodDelivery.Common;
using FoodDelivery.DTOs.Restaurant;
using FoodDelivery.Entities;
using FoodDelivery.Repositories.Interfaces;
using FoodDelivery.Service.Interfaces;

namespace FoodDelivery.Service.Implementations;
public class RestaurantService : IRestaurantService
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly FoodContext _context;

    public RestaurantService(IRestaurantRepository restaurantRepository, FoodContext context)
    {
        _restaurantRepository = restaurantRepository;
        _context = context;
    }

    // Helper: lấy quán của user đang đăng nhập
    private async Task<Restaurant?> GetRestaurantOfUser(Guid userId)
    {
        return await _restaurantRepository.GetRestaurantByUserIdAsync(userId);
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
}
