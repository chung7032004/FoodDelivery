using FoodDelivery.DTOs.Shipping;
namespace FoodDelivery.Service.Interfaces.IShipping;
public interface ISurchargePipeline
{
    Task<decimal> ApplyAsync(decimal fee, RouteInfo route, ShippingFeeContext ctx, CancellationToken ct);
}