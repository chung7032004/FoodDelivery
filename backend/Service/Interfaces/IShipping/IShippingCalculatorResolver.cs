using FoodDelivery.Common;
namespace FoodDelivery.Service.Interfaces.IShipping;
public interface IShippingCalculatorResolver
{
    Result<IShippingFeeCalculator> Resolve(string serviceType);
}