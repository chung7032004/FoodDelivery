namespace FoodDelivery.Constants;

public static class RoleIds
{
    public static readonly Guid Admin           = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid RestaurantOwner = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid RestaurantStaff = Guid.Parse("33333333-3333-3333-3333-333333333333");
    public static readonly Guid Shipper         = Guid.Parse("44444444-4444-4444-4444-444444444444");
    public static readonly Guid Customer        = Guid.Parse("55555555-5555-5555-5555-555555555555");
}