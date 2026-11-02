using FoodDelivery.Constants;
using FoodDelivery.Service.StateMachine;

namespace FoodDelivery.Service.Implementations.StateMachine;

/// Cầu nối giữa RoleIds (hạ tầng) và ActorRole (nghiệp vụ).
/// Tầng ngoài (Controller / Middleware / Service) dùng class này.
public static class ActorRoleMapper
{
    public static ActorRole? FromRoleId(Guid roleId)
    {
        if (roleId == RoleIds.Admin)           return ActorRole.Admin;
        if (roleId == RoleIds.Customer)        return ActorRole.Customer;
        if (roleId == RoleIds.Shipper)         return ActorRole.Shipper;
        if (roleId == RoleIds.RestaurantOwner) return ActorRole.RestaurantOwner;
        if (roleId == RoleIds.RestaurantStaff) return ActorRole.RestaurantStaff;
        return null;
    }

    public static ActorRole? FromRoleName(string roleName) => roleName switch
    {
        "Admin"           => ActorRole.Admin,
        "Customer"        => ActorRole.Customer,
        "Shipper"         => ActorRole.Shipper,
        "RestaurantOwner" => ActorRole.RestaurantOwner,
        "RestaurantStaff" => ActorRole.RestaurantStaff,
        _                 => null
    };

    /// Map 1 collection role name (từ Claim) → ActorRole.
    /// Ưu tiên theo thứ tự: Admin > RestaurantOwner > RestaurantStaff > Shipper > Customer.
    public static ActorRole? FromRoleNames(IEnumerable<string> roleNames)
    {
        var roles = roleNames
            .Select(FromRoleName)
            .Where(r => r.HasValue)
            .Select(r => r!.Value)
            .ToList();

        if (roles.Contains(ActorRole.Admin))           return ActorRole.Admin;
        if (roles.Contains(ActorRole.RestaurantOwner)) return ActorRole.RestaurantOwner;
        if (roles.Contains(ActorRole.RestaurantStaff)) return ActorRole.RestaurantStaff;
        if (roles.Contains(ActorRole.Shipper))         return ActorRole.Shipper;
        if (roles.Contains(ActorRole.Customer))        return ActorRole.Customer;
        return null;
    }
}