using FoodDelivery.Common;
using FoodDelivery.DTOs.Order;
using FoodDelivery.DTOs.Restaurant;

namespace FoodDelivery.Service.Interfaces;
public interface IRestaurantService
{
    Task<Result<RestaurantProfileDto>> GetMyRestaurantAsync(Guid userId);
    Task<Result<RestaurantStatusResponse>> GetStatusRestaurantAsync(Guid userId);
    Task<Result> OpenRestaurantAsync(Guid userId);
    Task<Result> CloseRestaurantAsync(Guid userId, CloseRestaurantRequest request);
    Task<Result> UpdateRestaurantAsync(Guid userId, UpdateRestaurantProfileRequest request);
    Task<Result> RegisterRestaurantAsync(Guid userId, RegisterRestaurantRequest request);
}
