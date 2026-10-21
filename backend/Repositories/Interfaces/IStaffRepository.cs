using FoodDelivery.DTOs.Staff;
using FoodDelivery.DTOs.Review;
using FoodDelivery.Entities;

namespace FoodDelivery.Repositories.Interfaces;

public interface IStaffRepository
{

    Task<Staff?> GetStaffByUserId(Guid userId);
    Task<List<OrderDto>> GetOrdersByRestaurant(Guid restaurantId);
    Task<List<OrderDto>> GetPendingOrdersByRestaurant(Guid restaurantId);
    Task<List<OrderDto>> GetCompletedOrdersByRestaurant(Guid restaurantId, int days = 30);
    Task<OrderDto?> GetOrderDetails(Guid orderId, Guid restaurantId);
    Task<bool> UpdateOrderStatusAsync(Guid orderId, Guid restaurantId, int newStatus);
    Task<Staff?> GetStaffByRestaurantIdAsync(Guid restaurantId);
    Task<bool> ReportOrderIssueAsync(Guid orderId, Guid restaurantId, string issueDescription, string issueType);
    Task<List<ReviewDto>> GetRestaurantReviewsAsync(Guid restaurantId, int? rating = null);
    Task<bool> IsStaffOfRestaurantAsync(Guid userId, Guid restaurantId);
}
