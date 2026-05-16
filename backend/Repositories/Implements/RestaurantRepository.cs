using FoodDelivery.Entities;
using FoodDelivery.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Repositories.Implementations;
public class RestaurantRepository : IRestaurantRepository
{
    private readonly FoodContext _context;
    public RestaurantRepository(FoodContext context)
    {
        _context = context;
    }

    // Lấy quán theo Id
    public async Task<Restaurant?> GetByIdAsync(Guid restaurantId)
    {
        return await _context.Restaurants
            .FirstOrDefaultAsync(r => r.Id == restaurantId);
    }

    // Lấy quán mà user đang quản lý (qua bảng Staff)
    public async Task<Restaurant?> GetRestaurantByUserIdAsync(Guid userId)
    {
        return await _context.Staff
            .Where(s => s.UserId == userId && s.IsActive)
            .Select(s => s.Restaurant)
            .FirstOrDefaultAsync();
    }

    public async Task<Restaurant?> GetStatusByIdAsync(Guid restaurantId)
    {
        return await _context.Restaurants
            .AsNoTracking()
            .Where(r => r.Id == restaurantId)
            .Select(r => new Restaurant
            {
                Id = r.Id,
                IsOpen = r.IsOpen,
                OpenTime = r.OpenTime,
                CloseTime = r.CloseTime,
                ClosingMessage = r.ClosingMessage,
                UpdatedAt = r.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(Restaurant restaurant)
    {
        _context.Update(restaurant);
        await Task.CompletedTask;
    }
}