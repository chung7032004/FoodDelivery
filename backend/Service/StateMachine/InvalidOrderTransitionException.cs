namespace FoodDelivery.Service.StateMachine;

public sealed class InvalidOrderTransitionException : Exception
{
    public OrderStatus From { get; }
    public OrderAction Action { get; }
    public ActorRole Actor { get; }

    public InvalidOrderTransitionException(OrderStatus from, OrderAction action, ActorRole actor)
        : base($"Không thể thực hiện '{action}' bởi '{actor}' khi đơn ở trạng thái '{from}'.")
    {
        From = from;
        Action = action;
        Actor = actor;
    }
}