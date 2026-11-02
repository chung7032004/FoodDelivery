using FoodDelivery.Entities;
using FoodDelivery.Service.Interfaces;
using FoodDelivery.Service.StateMachine;

namespace FoodDelivery.Service.Implements.StateMachine;

public sealed class OrderStateMachine : IOrderStateMachine
{
    // ================================================================
    // TRANSITION TABLE — NGUỒN SỰ THẬT DUY NHẤT
    // Đọc bảng này là biết toàn bộ vòng đời đơn hàng.
    // ================================================================
    private static readonly IReadOnlyList<OrderTransition> Transitions = new List<OrderTransition>
    {
        // ==================== Pending ====================
        // Confirm: cả Owner và Staff
        new(OrderStatus.Pending, OrderStatus.Confirmed,  OrderAction.Confirm,  ActorRole.RestaurantOwner),
        new(OrderStatus.Pending, OrderStatus.Confirmed,  OrderAction.Confirm,   ActorRole.RestaurantStaff),

        // ReportOutOfStock: cả Owner và Staff
        new(OrderStatus.Pending, OrderStatus.WaitingCustomerDecision, OrderAction.ReportOutOfStock,  ActorRole.RestaurantOwner),
        new(OrderStatus.Pending, OrderStatus.WaitingCustomerDecision, OrderAction.ReportOutOfStock,  ActorRole.RestaurantStaff),

        // Customer hủy
        new(OrderStatus.Pending, OrderStatus.Cancelled, OrderAction.CancelByCustomer,   ActorRole.Customer),

        // Admin hủy
        new(OrderStatus.Pending, OrderStatus.Cancelled, OrderAction.CancelByAdmin,      ActorRole.Admin),

        // Nhà hàng hủy — CHỈ OWNER
        new(OrderStatus.Pending, OrderStatus.Cancelled, OrderAction.CancelByRestaurant, ActorRole.RestaurantOwner),

        // ==================== WaitingCustomerDecision ====================
        new(OrderStatus.WaitingCustomerDecision, OrderStatus.Confirmed, OrderAction.AcceptProposal,     ActorRole.Customer),
        new(OrderStatus.WaitingCustomerDecision, OrderStatus.Cancelled, OrderAction.RejectProposal,     ActorRole.Customer),
        new(OrderStatus.WaitingCustomerDecision, OrderStatus.Cancelled, OrderAction.CancelByCustomer,   ActorRole.Customer),
        new(OrderStatus.WaitingCustomerDecision, OrderStatus.Cancelled, OrderAction.CancelByAdmin,      ActorRole.Admin),
        new(OrderStatus.WaitingCustomerDecision, OrderStatus.Cancelled, OrderAction.CancelByRestaurant, ActorRole.RestaurantOwner),

        // ==================== Confirmed ====================
        // StartPreparing: cả Owner và Staff
        new(OrderStatus.Confirmed, OrderStatus.Preparing, OrderAction.StartPreparing,   ActorRole.RestaurantOwner),
        new(OrderStatus.Confirmed, OrderStatus.Preparing, OrderAction.StartPreparing,   ActorRole.RestaurantStaff),

        new(OrderStatus.Confirmed, OrderStatus.Cancelled, OrderAction.CancelByCustomer,   ActorRole.Customer),
        new(OrderStatus.Confirmed, OrderStatus.Cancelled, OrderAction.CancelByAdmin,      ActorRole.Admin),
        new(OrderStatus.Confirmed, OrderStatus.Cancelled, OrderAction.CancelByRestaurant, ActorRole.RestaurantOwner),

        // ==================== Preparing ====================
        // MarkReady: cả Owner và Staff
        new(OrderStatus.Preparing, OrderStatus.ReadyForPickup, OrderAction.MarkReady,            ActorRole.RestaurantOwner),
        new(OrderStatus.Preparing, OrderStatus.ReadyForPickup, OrderAction.MarkReady,            ActorRole.RestaurantStaff),

        new(OrderStatus.Preparing, OrderStatus.Cancelled,      OrderAction.CancelByAdmin,        ActorRole.Admin),
        new(OrderStatus.Preparing, OrderStatus.Cancelled,      OrderAction.CancelByRestaurant,   ActorRole.RestaurantOwner),

        // ==================== ReadyForPickup ====================
        new(OrderStatus.ReadyForPickup, OrderStatus.Shipping,  OrderAction.Pickup,        ActorRole.Shipper),
        new(OrderStatus.ReadyForPickup, OrderStatus.Cancelled, OrderAction.CancelByAdmin, ActorRole.Admin),

        // ==================== Shipping ====================
        new(OrderStatus.Shipping, OrderStatus.Completed, OrderAction.Deliver,       ActorRole.Shipper),
        new(OrderStatus.Shipping, OrderStatus.Cancelled, OrderAction.CancelByAdmin, ActorRole.Admin),

        // ==================== Completed / Cancelled ====================
        // Terminal — không có transition ra.
    };

    // Index để tra cứu nhanh O(1)
    private static readonly ILookup<OrderStatus, OrderTransition> ByFrom =
        Transitions.ToLookup(t => t.From);

    // ================================================================
    // PUBLIC API
    // ================================================================

    public bool CanTransition(OrderStatus from, OrderAction action, ActorRole actor, Order order)
    {
        var match = FindTransition(from, action, actor);
        return match is not null && (match.Guard is null || match.Guard(order));
    }

    public OrderStatus GetNextStatus(OrderStatus from, OrderAction action, ActorRole actor, Order order)
    {
        var match = FindTransition(from, action, actor);
        if (match is null || (match.Guard is not null && !match.Guard(order)))
            throw new InvalidOrderTransitionException(from, action, actor);

        return match.To;
    }

    public IReadOnlyList<OrderAction> GetAllowedActions(OrderStatus from, ActorRole actor, Order order)
    {
        return ByFrom[from]
            .Where(t => t.Actor == actor)
            .Where(t => t.Guard is null || t.Guard(order))
            .Select(t => t.Action)
            .Distinct()
            .ToList();
    }


    private static OrderTransition? FindTransition(OrderStatus from, OrderAction action, ActorRole actor)
    => ByFrom[from].FirstOrDefault(t => t.Action == action && t.Actor == actor);
}