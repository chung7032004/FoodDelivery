namespace FoodDelivery.DTOs.Shipping;
public class ShippingFeeContext
{
    public double RestaurantLat { get; set; }
    public double RestaurantLon { get; set; }
    public double CustomerLat { get; set; }
    public double CustomerLon { get; set; }
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public decimal Subtotal { get; set; }
    public int ItemCount { get; set; }
    public Guid RestaurantId { get; set; }
}