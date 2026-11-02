namespace FoodDelivery.Service.StateMachine;

public static class ActorRoleExtensions
{
    public static bool IsRestaurantSide(this ActorRole role)
        => role is ActorRole.RestaurantOwner or ActorRole.RestaurantStaff;

    public static bool IsOwner(this ActorRole role)
        => role == ActorRole.RestaurantOwner;

    public static bool IsStaff(this ActorRole role)
        => role == ActorRole.RestaurantStaff;

    public static bool IsAdmin(this ActorRole role)
        => role == ActorRole.Admin;

    public static bool IsShipper(this ActorRole role)
        => role == ActorRole.Shipper;

    public static bool IsCustomer(this ActorRole role)
        => role == ActorRole.Customer;

    public static bool IsSystem(this ActorRole role)
        => role == ActorRole.System;
}