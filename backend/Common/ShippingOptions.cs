namespace FoodDelivery.Common.Options;

public class ShippingOptions
{
    public const string SectionName = "Shipping";

    public double MaxDistanceKm { get; set; } = 40;
    public decimal MaxShippingFee { get; set; } = 150_000m;
    public decimal RoundUnit { get; set; } = 500m;

    // Cấu hình base fee theo dịch vụ
    public StandardFeeOptions Standard { get; set; } = new();
    public ExpressFeeOptions Express { get; set; } = new();

    public class StandardFeeOptions
    {
        public decimal BaseFee { get; set; } = 10_000m;
        public decimal PerKm { get; set; } = 4_000m;
    }
    public class ExpressFeeOptions
    {
        public decimal BaseFee { get; set; } = 20_000m;
        public decimal PerKm { get; set; } = 6_000m;
    }
}