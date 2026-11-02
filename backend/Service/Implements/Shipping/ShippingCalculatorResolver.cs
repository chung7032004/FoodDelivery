using FoodDelivery.Common;
using FoodDelivery.Service.Implements.Shipping;
using FoodDelivery.Service.Interfaces.IShipping;

public class ShippingCalculatorResolver : IShippingCalculatorResolver
{
    private readonly Dictionary<string, Type> _map = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Standard"] = typeof(StandardShippingCalculator),
        ["Express"] = typeof(ExpressShippingCalculator),
    };
    private readonly IServiceProvider _sp;

    public ShippingCalculatorResolver(IServiceProvider sp) => _sp = sp;

    public Result<IShippingFeeCalculator> Resolve(string serviceType)
    {
        if (!_map.TryGetValue(serviceType, out var type))
            return HttpErrors.NotFound<IShippingFeeCalculator>("UNKNOWN_SERVICE",$"Không hỗ trợ '{serviceType}'");
        return Result<IShippingFeeCalculator>.Success(
            (IShippingFeeCalculator)_sp.GetRequiredService(type));
    }
}