using FoodDelivery.Entities;

namespace FoodDelivery.Repositories.Interfaces;
public interface IRestaurantRepository
{
    Task<Restaurant?> GetByIdAsync(Guid restaurantId);
    Task<Restaurant?> GetRestaurantByUserIdAsync(Guid userId);
    Task<Restaurant?> GetStatusByIdAsync(Guid restaurantId);
    Task<(List<Restaurant> Items, int TotalCount)> GetAllForAdminAsync(int page, int pageSize, RestaurantApplicationStatus? status);
    Task UpdateAsync(Restaurant restaurant);
}