using FoodDelivery.DTOs.Order;
using FoodDelivery.DTOs.Restaurant;
using FoodDelivery.Entities;
using FoodDelivery.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Repositories.Implementations;
public class RestaurantRepository : IRestaurantRepository
{
    private readonly FoodContext _context;
    public RestaurantRepository (FoodContext context)
    {
        _context = context;
    }
    public async Task<Restaurant?> GetMyRestaurant()
    {
        return await _context.Restaurants.FirstOrDefaultAsync();   
    }
    public async Task<Restaurant?> GetStatusRestaurantAsync()
    {
        return await _context.Restaurants
            .AsNoTracking()
            .Select(r => new Restaurant
            {
                IsOpen = r.IsOpen,
                OpenTime = r.OpenTime,
                CloseTime = r.CloseTime,
                ClosingMessage = r.ClosingMessage,
                UpdatedAt = r.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }
    public async Task UpdateAsync (Restaurant restaurant)
    {
        _context.Update(restaurant);
        await Task.CompletedTask;
    }
}