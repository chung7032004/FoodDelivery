using FoodDelivery.Common;
using FoodDelivery.DTOs.Account;

namespace FoodDelivery.Service.Interfaces;

public interface IUserRoleAccessService
{
    Task<Result> SuspendRoleAsync(Guid userId, Guid adminId, string roleName, SuspendUserRoleRequest request);
    Task<Result<List<UserRoleRestrictionResponse>>> GetMyRestrictionsAsync(Guid userId);
    Task<Result> SubmitAppealAsync(Guid userId, SubmitRoleAppealRequest request);
    Task<Result<List<AdminRoleAppealResponse>>> GetPendingAppealsAsync();
    Task<Result> ReviewAppealAsync(Guid appealId, Guid adminId, ReviewRoleAppealRequest request);
}
