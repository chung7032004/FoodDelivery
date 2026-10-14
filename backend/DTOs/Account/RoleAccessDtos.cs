namespace FoodDelivery.DTOs.Account;

public class SuspendUserRoleRequest
{
    public string Reason { get; set; } = string.Empty;
}

public class SubmitRoleAppealRequest
{
    public string RoleName { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
}

public class ReviewRoleAppealRequest
{
    public bool IsApproved { get; set; }
    public string? Response { get; set; }
}

public class UserRoleRestrictionResponse
{
    public Guid RestrictionId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public DateTime RestrictedAt { get; set; }
    public Guid? AppealId { get; set; }
    public string? AppealReason { get; set; }
    public string? AppealStatus { get; set; }
    public DateTime? AppealSubmittedAt { get; set; }
    public string? AppealResponse { get; set; }
}

public class AdminRoleAppealResponse
{
    public Guid AppealId { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public string RestrictionReason { get; set; } = string.Empty;
    public string AppealReason { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
}
