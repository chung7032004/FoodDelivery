using FoodDelivery.Common.Options;
using FoodDelivery.DTOs.Shipping;
using FoodDelivery.Service.Implements.Shipping;
using FoodDelivery.Service.Interfaces.IShipping;
using Microsoft.Extensions.Options;

public class StandardShippingCalculator : ShippingFeeCalculatorTemplate
{
    public StandardShippingCalculator(IRoutingService r, ISurchargePipeline s, IOptions<ShippingOptions> o)
        : base(r, s, o) { }
    protected override decimal CalculateBaseFee(RouteInfo route, ShippingFeeContext ctx)
        => _opt.Standard.BaseFee + (decimal)route.DistanceKm * _opt.Standard.PerKm;
}