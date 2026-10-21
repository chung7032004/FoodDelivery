using FoodDelivery.Entities;
using FoodDelivery.Service.StateMachine;

namespace FoodDelivery.Service.Interfaces;

public interface IOrderStateMachine
{
    /// Kiểm tra transition có hợp lệ không (bao gồm cả Guard)
    bool CanTransition(OrderStatus from, OrderAction action, ActorRole actor, Order order);

    /// Lấy status đích. Throw InvalidOrderTransitionException nếu không hợp lệ.
    OrderStatus GetNextStatus(OrderStatus from, OrderAction action, ActorRole actor, Order order);

    /// Lấy danh sách action mà actor được phép làm ở status hiện tại.
    IReadOnlyList<OrderAction> GetAllowedActions(OrderStatus from, ActorRole actor, Order order);
}