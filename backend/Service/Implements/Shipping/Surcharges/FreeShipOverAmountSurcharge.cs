using FoodDelivery.DTOs.Shipping;
using FoodDelivery.Service.Interfaces.IShipping;

namespace FoodDelivery.Service.Implements.Shipping;
public class FreeShipOverAmountSurcharge : ISurchargeDecorator
{
    private readonly ISurchargeDecorator? _inner;
    private readonly decimal _threshold = 500_000m;
    public FreeShipOverAmountSurcharge(ISurchargeDecorator? inner = null) => _inner = inner;
     public async Task<decimal> ApplyAsync(decimal fee, RouteInfo r, ShippingFeeContext c, CancellationToken ct)
    {
        if (c.Subtotal >= _threshold) return 0m;
        return _inner is null ? fee : await _inner.ApplyAsync(fee, r, c, ct);
    }
}