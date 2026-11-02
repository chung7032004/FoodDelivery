namespace FoodDelivery.DTOs.Shipping;
public record RouteInfo(
    double DistanceKm,
    int DurationMinutes,
    string Provider,
    bool IsFallback = false);