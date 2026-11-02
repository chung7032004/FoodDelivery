using FoodDelivery.Common;
using FoodDelivery.DTOs.Shipping;
using FoodDelivery.Entities;

namespace FoodDelivery.Service.Interfaces.IShipping;
public interface IShippingFeeCalculator
{
    Task<Result<ShippingFeeResult>> CalculateAsync(ShippingFeeContext ctx, CancellationToken ct = default);
}