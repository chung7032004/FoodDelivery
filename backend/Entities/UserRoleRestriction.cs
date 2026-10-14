namespace FoodDelivery.Entities;

public class UserRoleRestriction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public Guid RestrictedByAdminId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime RestrictedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    public ICollection<UserRoleAppeal> Appeals { get; set; } = new List<UserRoleAppeal>();
}

public class UserRoleAppeal
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid RestrictionId { get; set; }
    public UserRoleRestriction Restriction { get; set; } = null!;
    public string Reason { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public RoleAppealStatus Status { get; set; } = RoleAppealStatus.Pending;
    public Guid? ReviewedByAdminId { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public string? AdminResponse { get; set; }
}
