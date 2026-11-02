using FoodDelivery.Common;
using FoodDelivery.DTOs.Shipping;
using FoodDelivery.Entities;
using FoodDelivery.Repositories.Interfaces;
using FoodDelivery.Service.Interfaces.IShipping;

public class ShippingEstimationService : IShippingEstimationService
{
    private readonly IShippingCalculatorResolver _resolver;
    private readonly IAddressRepository _addressRepo;
    private readonly IRestaurantRepository _restaurantRepo;

    public ShippingEstimationService(
        IShippingCalculatorResolver resolver,
        IAddressRepository addressRepo,
        IRestaurantRepository restaurantRepo)
    {
        _resolver = resolver;
        _addressRepo = addressRepo;
        _restaurantRepo = restaurantRepo;
    }

    public async Task<Result<ShippingFeeResult>> EstimateAsync(
        Guid addressId, Guid restaurantId, string serviceType,
        decimal subtotal, int itemCount, CancellationToken ct = default)
    {
        var calcResult = _resolver.Resolve(serviceType);
        if (!calcResult.IsSuccess)
            return Result<ShippingFeeResult>.Failure(
                calcResult.ErrorCode
                    ?? throw new InvalidOperationException("Shipping calculator resolution failed without an error code."),
                calcResult.Message
                    ?? throw new InvalidOperationException("Shipping calculator resolution failed without an error message."),
                calcResult.StatusCode);

        var calculator = calcResult.Data
            ?? throw new InvalidOperationException("Shipping calculator resolution returned no calculator.");

        var address = await _addressRepo.GetByIdAsync(addressId);
        var restaurant = await _restaurantRepo.GetByIdAsync(restaurantId);
        if (address is null || restaurant is null)
            return HttpErrors.NotFound<ShippingFeeResult>("NOT_FOUND", "Address/Restaurant not found");

        var ctx = new ShippingFeeContext
        {
            RestaurantLat = restaurant.Latitude,
            RestaurantLon = restaurant.Longitude,
            CustomerLat = address.Latitude,
            CustomerLon = address.Longitude,
            Subtotal = subtotal,
            ItemCount = itemCount,
            RestaurantId = restaurantId,
            RequestedAt = DateTime.UtcNow
        };

        return await calculator.CalculateAsync(ctx, ct);
    }
}