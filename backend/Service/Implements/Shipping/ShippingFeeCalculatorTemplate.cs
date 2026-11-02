using FoodDelivery.Common;
using FoodDelivery.Common.Options;
using FoodDelivery.DTOs.Shipping;
using FoodDelivery.Service.Interfaces.IShipping;
using Microsoft.Extensions.Options;

namespace FoodDelivery.Service.Implements.Shipping;
public abstract class ShippingFeeCalculatorTemplate : IShippingFeeCalculator
{
    protected readonly IRoutingService _routing;
    protected readonly ISurchargePipeline _surcharges;
    protected readonly ShippingOptions _opt;

    protected ShippingFeeCalculatorTemplate(
        IRoutingService routing,
        ISurchargePipeline surcharges,
        IOptions<ShippingOptions> opt)
    { _routing = routing; _surcharges = surcharges; _opt = opt.Value; }
    public async Task<Result<ShippingFeeResult>> CalculateAsync(ShippingFeeContext ctx, CancellationToken ct = default)
    {
        // Bước 1: Routing
        var route = await _routing.GetRouteAsync(ctx.RestaurantLat, ctx.RestaurantLon,ctx.CustomerLat, ctx.CustomerLon, ct);
        RouteInfo routeInfo;
        if(route.IsSuccess && route.Data != null)
        {
            routeInfo = route.Data;
        }
        else
        {
            // Fallback Haversine × 1.33
            var km = Haversine(ctx.RestaurantLat, ctx.RestaurantLon, ctx.CustomerLat, ctx.CustomerLon) * 1.33;
            routeInfo = new RouteInfo(km, (int)(km * 4), "HaversineFallback", IsFallback: true);
        }
        if (routeInfo.DistanceKm > _opt.MaxDistanceKm)
            return HttpErrors.BadRequest<ShippingFeeResult>(
                "TOO_FAR",
                $"Khoảng cách giao hàng vượt quá {_opt.MaxDistanceKm} km.");
        if (_opt.RoundUnit <= 0)
            throw new InvalidOperationException("Shipping:RoundUnit must be greater than zero.");

        // Bước 2: Base fee — strategy con override
        var baseFee = CalculateBaseFee(routeInfo, ctx);
        // Bước 3: Surcharge pipeline (Decorator)
         var finalFee = await _surcharges.ApplyAsync(baseFee, routeInfo, ctx, ct);
        // Bước 4: Làm tròn
        finalFee = Math.Ceiling(finalFee / _opt.RoundUnit) * _opt.RoundUnit;
        // Bước 5: Cap
        finalFee = Math.Min(finalFee, _opt.MaxShippingFee);
        return Result<ShippingFeeResult>.Success(new ShippingFeeResult(finalFee, routeInfo));
    }
    protected abstract decimal CalculateBaseFee(RouteInfo route, ShippingFeeContext ctx);
    private static double Haversine(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371;
        var dLat = (lat1 - lat2) * Math.PI / 180;
        var dLon = (lon1 - lon2) * Math.PI / 180;
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
              + Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180)
              * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        return R * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
    }
}