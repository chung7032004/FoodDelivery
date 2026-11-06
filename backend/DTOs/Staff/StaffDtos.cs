namespace FoodDelivery.DTOs.Staff;

public class OrderDto
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = "";
    public string CustomerName { get; set; } = "";
    public string CustomerPhone { get; set; } = "";
    public string? ShipperName { get; set; }
    public string? ShipperPhone { get; set; }
    public string? ShipperVehicleType { get; set; }
    public string? ShipperVehiclePlate { get; set; }
    public decimal TotalAmount { get; set; }
    public int Status { get; set; }
    public int? PrepMinutes { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}

public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public class ReportOutOfStockRequest
{
    public List<Guid> RemovedProductIds { get; set; } = new();
    public string Note { get; set; } = string.Empty;
}

public class StaffStatsDto
{
    public int TotalOrdersToday { get; set; }
    public int PreparingCount { get; set; }
    public int ReadyCount { get; set; }
    public int WaitingCustomerDecisionCount { get; set; }
    public int PendingCount { get; set; }
    public int ConfirmedCount { get; set; }
    public int ShippingCount { get; set; }
}

public class RestaurantAnalyticsPointDto
{
    public DateTime PeriodStart { get; set; }
    public decimal Revenue { get; set; }
    public decimal? GrossProfit { get; set; }
    public int CompletedOrders { get; set; }
    public int OrdersMissingCost { get; set; }
}

public class RestaurantAnalyticsDto
{
    public string Period { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public decimal? GrossProfit { get; set; }
    public int CompletedOrders { get; set; }
    public int OrdersMissingCost { get; set; }
    public List<RestaurantAnalyticsPointDto> Points { get; set; } = new();
}

public class RestaurantAnalyticsOrderDto
{
    public DateTime CompletedAt { get; set; }
    public decimal Revenue { get; set; }
    public decimal? GrossProfit { get; set; }
}

public class UpdateOrderStatusRequest
{
    public int Status { get; set; }
}

public class ReportIssueRequest
{
    public string IssueDescription { get; set; } = "";
    public string IssueType { get; set; } = ""; // OutOfStock, Damaged, etc.
}

public class StaffProfileDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid RestaurantId { get; set; }
    public string Position { get; set; } = "";
    public bool IsActive { get; set; }
    public string RestaurantName { get; set; } = "";
    public string UserName { get; set; } = "";
}
