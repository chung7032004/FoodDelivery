using FoodDelivery.Constants;

namespace FoodDelivery.DTOs.Order;
public class BuyNowRequestDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public Guid AddressId { get; set; }
    public string ShippingService { get; set; } = "Standard";
    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
    public string? Note {get; set;} = string.Empty;
}