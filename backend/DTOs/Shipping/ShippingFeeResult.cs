namespace FoodDelivery.DTOs.Shipping;
public record ShippingFeeResult(decimal Fee, RouteInfo Route)
{
    public static ShippingFeeResult Fail(string code, string message) => throw new NotImplementedException();
}