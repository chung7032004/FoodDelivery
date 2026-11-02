using FoodDelivery.DTOs.Shipping;
using FoodDelivery.Entities;
using FoodDelivery.Service.Interfaces.IShipping;
namespace FoodDelivery.Service.Implements.Shipping.Surcharges;
public class PeakHourSurcharge : ISurchargeDecorator
{
    private readonly ISurchargeDecorator? _inner;
    private static readonly (int s, int e)[] Peaks = { (11, 13), (17, 20) };
    public PeakHourSurcharge(ISurchargeDecorator? inner = null) => _inner = inner;

    public async Task<decimal> ApplyAsync(decimal fee, RouteInfo r, ShippingFeeContext c, CancellationToken ct)
    {
        var f = _inner is null ? fee : await _inner.ApplyAsync(fee, r, c, ct);
        var h = c.RequestedAt.Hour;
        if (Peaks.Any(p => h >= p.s && h < p.e)) f *= 1.2m;
        return f;
    }
}