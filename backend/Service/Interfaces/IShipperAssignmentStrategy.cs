namespace FoodDelivery.Service.Interfaces;
public interface IShipperAssignmentStrategy
{
    Task<Guid?> FindShipperAsync(Guid orderId, Guid restaurantId);
}