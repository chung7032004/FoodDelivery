using FoodDelivery.DTOs.Shipping;
using FoodDelivery.Service.Implements.Shipping.Surcharges;
using FoodDelivery.Service.Interfaces.IShipping;
namespace FoodDelivery.Service.Implements.Shipping;
public class SurchargePipeline : ISurchargePipeline
{
    private readonly ISurchargeDecorator _root;
    public SurchargePipeline()
    {
        _root = new FreeShipOverAmountSurcharge(
            new PeakHourSurcharge(
                new NightSurcharge()
            )
        );
    }
    public Task<decimal> ApplyAsync(decimal fee, RouteInfo r, ShippingFeeContext c, CancellationToken ct)
    {
        return _root.ApplyAsync(fee, r, c, ct);
    }
}