using FoodDelivery.Entities;

namespace FoodDelivery.Service.StateMachine;

/// Một dòng trong Transition Table.
/// Đại diện cho 1 mũi tên: From ──(Action by Actor)──> To
/// <param name="From">Trạng thái nguồn</param>
/// <param name="To">Trạng thái đích</param>
/// <param name="Action">Hành động</param>
/// <param name="Actor">Ai được phép thực hiện</param>
/// <param name="Guard">Điều kiện phụ (optional) — nếu có, phải thỏa mới transition được</param>
public sealed record OrderTransition(
    OrderStatus From,
    OrderStatus To,
    OrderAction Action,
    ActorRole Actor,
    Func<Order, bool>? Guard = null
);