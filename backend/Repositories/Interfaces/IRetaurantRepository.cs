using FoodDelivery.Entities;

namespace FoodDelivery.Repositories.Interfaces;
public interface IRestaurantRepository
{
    Task<Restaurant?> GetByIdAsync(Guid restaurantId);
    Task<Restaurant?> GetRestaurantByUserIdAsync(Guid userId);
    Task<Restaurant?> GetStatusByIdAsync(Guid restaurantId);
    Task UpdateAsync(Restaurant restaurant);
}