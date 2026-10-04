using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.DTOs.Restaurant;

public class RegisterRestaurantRequest
{
    [Required, StringLength(120, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;
    [Required, StringLength(300, MinimumLength = 5)]
    public string Address { get; set; } = string.Empty;
    [Required, RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải gồm 10 chữ số và bắt đầu bằng 0")]
    public string Phone { get; set; } = string.Empty;
    [Range(-90, 90)]
    public double Latitude { get; set; }
    [Range(-180, 180)]
    public double Longitude { get; set; }
    public TimeSpan OpenTime { get; set; }
    public TimeSpan CloseTime { get; set; }
}

public class ReviewRestaurantRequest
{
    public bool IsApproved { get; set; }
    [StringLength(500)]
    public string? RejectReason { get; set; }
}

public class RestaurantApplicationResponse
{
    public Guid RestaurantId { get; set; }
    public Guid OwnerId { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public string OwnerEmail { get; set; } = string.Empty;
    public string OwnerPhone { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public TimeSpan OpenTime { get; set; }
    public TimeSpan CloseTime { get; set; }
    public bool IsOpen { get; set; }
    public RestaurantApplicationStatus ApplicationStatus { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
}

public class MyRestaurantApplicationResponse
{
    public bool HasApplication { get; set; }
    public RestaurantApplicationResponse? Application { get; set; }
}
