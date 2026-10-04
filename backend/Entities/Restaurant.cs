namespace FoodDelivery.Entities;

public class Restaurant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OwnerId { get; set; }          
    public virtual User? Owner { get; set; }    
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public RestaurantApplicationStatus ApplicationStatus { get; set; } = RestaurantApplicationStatus.Pending;
    public string? RejectionReason { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public Guid? ReviewedByAdminId { get; set; }
    public bool IsOpen { get; set; }
    public string? ClosingMessage { get; set; }
    public TimeSpan OpenTime { get; set; }
    public TimeSpan CloseTime { get; set; }

    // Vị trí
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
