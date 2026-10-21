namespace FoodDelivery.Service.StateMachine;

public enum ActorRole
{
    Customer,
    Admin,
    Shipper,
    RestaurantOwner,   // chủ nhà hàng — quyền cao nhất phía nhà hàng
    RestaurantStaff,   // nhân viên — chỉ nghiệp vụ đơn hàng
    System             // background job / auto action
}