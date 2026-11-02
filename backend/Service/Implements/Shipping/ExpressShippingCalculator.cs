using FoodDelivery.Common.Options;
using FoodDelivery.DTOs.Shipping;
using FoodDelivery.Service.Interfaces.IShipping;
using Microsoft.Extensions.Options;
namespace FoodDelivery.Service.Implements.Shipping;
public class ExpressShippingCalculator : ShippingFeeCalculatorTemplate
{
    public ExpressShippingCalculator(IRoutingService r, ISurchargePipeline s, IOptions<ShippingOptions> o)
        : base(r, s, o) { }
    protected override decimal CalculateBaseFee(RouteInfo route, ShippingFeeContext ctx)
        => _opt.Express.BaseFee + (decimal)route.DistanceKm * _opt.Express.PerKm;
}