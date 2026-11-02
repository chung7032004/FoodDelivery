using FoodDelivery.Common;
using FoodDelivery.DTOs.Shipping;
namespace FoodDelivery.Service.Interfaces.IShipping;
public interface IShippingEstimationService
{
    Task<Result<ShippingFeeResult>> EstimateAsync(
        Guid addressId, Guid restaurantId, string serviceType,
        decimal subtotal, int itemCount, CancellationToken ct = default);
}