namespace FoodDelivery.Service.StateMachine;

/// <summary>
/// Hành động có thể tác động lên đơn hàng.
/// Mỗi action tương ứng với 1 method nghiệp vụ trong OrderService.
/// </summary>
public enum OrderAction
{
    // Phía nhà hàng
    Confirm,              // Nhà hàng xác nhận đơn
    ReportOutOfStock,     // Nhà hàng báo hết món
    StartPreparing,       // Nhà hàng bắt đầu nấu
    MarkReady,            // Nhà hàng nấu xong, chờ shipper

    // Phía khách hàng
    AcceptProposal,       // Khách đồng ý với thay đổi của quán
    RejectProposal,       // Khách từ chối thay đổi
    CancelByCustomer,     // Khách hủy đơn

    // Phía shipper
    Pickup,               // Shipper lấy hàng
    Deliver,              // Shipper giao xong

    // Phía admin hệ thống
    CancelByAdmin,        // Admin hệ thống hủy đơn

    // Phía nhà hàng (quản trị) — chỉ Owner
    CancelByRestaurant    // Chủ nhà hàng hủy đơn
}