using FoodDelivery.Common;
using FoodDelivery.DTOs.Shipping;
using FoodDelivery.Service.Interfaces.IShipping;
using Microsoft.Extensions.Caching.Memory;
namespace FoodDelivery.Service.Implements.Shipping;
public class CachedRoutingService : IRoutingService
{
    private readonly IRoutingService _inner;
    private readonly IMemoryCache _cache;

    public CachedRoutingService(IRoutingService inner, IMemoryCache cache)
    { _inner = inner; _cache = cache; }

    public async Task<Result<RouteInfo>> GetRouteAsync(double a, double b, double c, double d, CancellationToken ct = default)
    {
        var key = $"route:{a:F4},{b:F4}:{c:F4},{d:F4}";
        if (_cache.TryGetValue(key, out RouteInfo? cached) && cached != null)
            return Result<RouteInfo>.Success(cached);

        var r = await _inner.GetRouteAsync(a, b, c, d, ct);
        if (r.IsSuccess) _cache.Set(key, r.Data, TimeSpan.FromMinutes(5));
        return r;
    }
}