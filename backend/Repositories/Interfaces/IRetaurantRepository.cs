using FoodDelivery.DTOs.Restaurant;
using FoodDelivery.Entities;

namespace FoodDelivery.Repositories.Interfaces;
public interface IRestaurantRepository
{
    Task<Restaurant?> GetMyRestaurant();
    Task<Restaurant?> GetStatusRestaurantAsync();
    Task UpdateAsync (Restaurant restaurant);
}