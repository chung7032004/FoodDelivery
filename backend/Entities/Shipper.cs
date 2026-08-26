namespace FoodDelivery.Entities
{
    public class Shipper
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // Thông tin nghiệp vụ shipper
        public string IdentityCard { get; set; } = string.Empty;
        public string VehicleType { get; set; } = string.Empty;
        public string VehiclePlate { get; set; } = string.Empty;

        // Trạng thái duyệt
        public ShipperStatus Status { get; set; } = ShipperStatus.Pending;
        public string? RejectReason { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public Guid? ReviewedByAdminId { get; set; }

        // Vận hành
        public bool IsActive { get; set; } = false;
        public bool IsAvailable { get; set; } = false;
        public int TotalDeliveredOrders { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<OrderDetail> Orders { get; set; } = new List<OrderDetail>();
    }
}