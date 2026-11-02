using FoodDelivery.DTOs.Shipping;
using FoodDelivery.Entities;
using FoodDelivery.Service.Interfaces.IShipping;
namespace FoodDelivery.Service.Implements.Shipping;
public class NightSurcharge : ISurchargeDecorator
{
    private readonly ISurchargeDecorator? _inner;
    public NightSurcharge(ISurchargeDecorator? inner = null) => _inner = inner;
    public async Task<decimal> ApplyAsync(decimal fee, RouteInfo r, ShippingFeeContext c, CancellationToken ct)
    {
        var f = _inner is null ? fee : await _inner.ApplyAsync(fee, r, c, ct);
        var h = c.RequestedAt.Hour;
        if (h >= 22 || h < 6) f += 10_000m;
        return f;
    }
}