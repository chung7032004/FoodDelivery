namespace FoodDelivery.Entities
{
    public class OrderDetail
    {
        public Guid Id {get; set;} =  Guid.NewGuid();
        public Guid OrderId {get; set;} 
        public Order Order {get;set;} = null!;
        public Guid? ShipperId { get; set; }
        public Shipper? Shipper { get; set; }
        public DateTime? ShipperAssignedAt { get; set; }
        public DispatchStatus DispatchStatus { get; set; } = DispatchStatus.NotStarted;
        public DateTime? DispatchStartedAt { get; set; }
        public int? PrepMinutes { get; set; }
        public int ShipperReleaseCount { get; set; }
        public string? LastShipperReleaseReason { get; set; }
        public bool ShipperCancellationCompensationPending { get; set; }
        public OrderStatus Status {get; set;} 
        public PaymentMethod PaymentMethod {get; set;} 
        public PaymentStatus PaymentStatus {get; set;} 
        public long RowVersion { get; set; }
        public Guid? CancelledByUserId { get; set; }
        public User? CancelledByUser { get; set; }
        public string CancelReason {get; set;} = string.Empty;
        public DateTime? EstimatedDeliveryTime {get; set;}
        public DateTime? ActualDeliveryTime {get; set;}
    }
}