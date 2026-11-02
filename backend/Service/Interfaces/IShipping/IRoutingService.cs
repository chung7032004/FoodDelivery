using FoodDelivery.Common;
using FoodDelivery.DTOs.Shipping;
namespace FoodDelivery.Service.Interfaces.IShipping;
public interface IRoutingService
{
    Task<Result<RouteInfo>> GetRouteAsync(double fromLat, double fromLon,double toLat, double toLon,CancellationToken ct = default);
}