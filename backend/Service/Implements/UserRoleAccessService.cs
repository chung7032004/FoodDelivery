using FoodDelivery.Common;
using FoodDelivery.Constants;
using FoodDelivery.DTOs.Account;
using FoodDelivery.DTOs.Notification;
using FoodDelivery.Entities;
using FoodDelivery.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Service.Implements;

public class UserRoleAccessService : IUserRoleAccessService
{
    private static readonly HashSet<string> RestrictableRoles = new(StringComparer.OrdinalIgnoreCase)
    {
        "Shipper",
        "RestaurantOwner"
    };

    private readonly FoodContext _context;
    private readonly INotificationService _notificationService;

    public UserRoleAccessService(FoodContext context, INotificationService notificationService)
    {
        _context = context;
        _notificationService = notificationService;
    }

    public async Task<Result> SuspendRoleAsync(Guid userId, Guid adminId, string roleName, SuspendUserRoleRequest request)
    {
        if (!RestrictableRoles.Contains(roleName))
            return HttpErrors.BadRequest("ROLE_NOT_RESTRICTABLE", "Chỉ có thể thu hồi quyền Shipper hoặc RestaurantOwner.");
        if (string.IsNullOrWhiteSpace(request.Reason) || request.Reason.Trim().Length < 5 || request.Reason.Trim().Length > 500)
            return HttpErrors.BadRequest("INVALID_REASON", "Lý do thu hồi quyền phải từ 5 đến 500 ký tự.");

        var canonicalRoleName = RestrictableRoles.Single(name => name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        var user = await _context.Users
            .Include(account => account.UserRoles)
            .FirstOrDefaultAsync(account => account.Id == userId);
        if (user == null)
            return HttpErrors.NotFound("USER_NOT_FOUND", "Không tìm thấy tài khoản.");
        if (user.UserRoles.Any(userRole => userRole.RoleId == RoleIds.Admin))
            return HttpErrors.BadRequest("ADMIN_ROLE_PROTECTED", "Không thể thu hồi quyền của tài khoản quản trị.");

        var role = await _context.Roles.FirstOrDefaultAsync(item => item.Name == canonicalRoleName);
        if (role == null)
            return HttpErrors.NotFound("ROLE_NOT_FOUND", "Không tìm thấy vai trò.");
        if (!user.UserRoles.Any(userRole => userRole.RoleId == role.Id))
            return HttpErrors.BadRequest("ROLE_NOT_ASSIGNED", "Tài khoản hiện không có vai trò này.");
        if (await _context.UserRoleRestrictions.AnyAsync(restriction =>
            restriction.UserId == userId && restriction.RoleId == role.Id && restriction.IsActive))
            return HttpErrors.BadRequest("ROLE_ALREADY_RESTRICTED", "Vai trò này đã bị thu hồi.");

        if (role.Name == "Shipper")
        {
            var shipper = await _context.Shippers.FirstOrDefaultAsync(item => item.UserId == userId);
            if (shipper != null)
            {
                shipper.IsActive = false;
                shipper.IsAvailable = false;
                shipper.UpdatedAt = DateTime.UtcNow;
            }
        }
        else
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(item => item.OwnerId == userId);
            if (restaurant != null)
            {
                restaurant.IsOpen = false;
                restaurant.UpdatedAt = DateTime.UtcNow;
            }
        }

        _context.UserRoles.Remove(user.UserRoles.Single(userRole => userRole.RoleId == role.Id));
        _context.UserRoleRestrictions.Add(new UserRoleRestriction
        {
            UserId = userId,
            RoleId = role.Id,
            RestrictedByAdminId = adminId,
            Reason = request.Reason.Trim(),
            RestrictedAt = DateTime.UtcNow,
            IsActive = true
        });
        await _context.SaveChangesAsync();
        await _notificationService.CreateNotificationAsync(userId, new NotificationRequest
        {
            Title = $"Quyền {canonicalRoleName} đã bị thu hồi",
            Message = $"Quyền {canonicalRoleName} của bạn đã bị thu hồi. Lý do: {request.Reason.Trim()}",
            Type = (int)NotificationType.SYSTEM,
            Link = "/account"
        });
        return Result.Success();
    }

    public async Task<Result<List<UserRoleRestrictionResponse>>> GetMyRestrictionsAsync(Guid userId)
    {
        var restrictions = await _context.UserRoleRestrictions
            .AsNoTracking()
            .Include(restriction => restriction.Role)
            .Include(restriction => restriction.Appeals)
            .Where(restriction => restriction.UserId == userId && restriction.IsActive)
            .OrderByDescending(restriction => restriction.RestrictedAt)
            .ToListAsync();

        var response = restrictions.Select(restriction =>
        {
            var latestAppeal = restriction.Appeals.OrderByDescending(appeal => appeal.SubmittedAt).FirstOrDefault();
            return new UserRoleRestrictionResponse
            {
                RestrictionId = restriction.Id,
                RoleName = restriction.Role.Name,
                Reason = restriction.Reason,
                RestrictedAt = restriction.RestrictedAt,
                AppealId = latestAppeal?.Id,
                AppealReason = latestAppeal?.Reason,
                AppealStatus = latestAppeal?.Status.ToString(),
                AppealSubmittedAt = latestAppeal?.SubmittedAt,
                AppealResponse = latestAppeal?.AdminResponse
            };
        }).ToList();
        return Result<List<UserRoleRestrictionResponse>>.Success(response);
    }

    public async Task<Result> SubmitAppealAsync(Guid userId, SubmitRoleAppealRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RoleName) || string.IsNullOrWhiteSpace(request.Reason)
            || request.Reason.Trim().Length < 10 || request.Reason.Trim().Length > 1000)
            return HttpErrors.BadRequest("INVALID_APPEAL", "Nội dung khiếu nại phải từ 10 đến 1000 ký tự.");

        var restriction = await _context.UserRoleRestrictions
            .Include(item => item.Role)
            .Include(item => item.Appeals)
            .FirstOrDefaultAsync(item =>
                item.UserId == userId &&
                item.IsActive &&
                item.Role.Name == request.RoleName);
        if (restriction == null)
            return HttpErrors.NotFound("ACTIVE_RESTRICTION_NOT_FOUND", "Không tìm thấy quyền đang bị thu hồi để khiếu nại.");
        if (restriction.Appeals.Any(appeal => appeal.Status == RoleAppealStatus.Pending))
            return HttpErrors.BadRequest("APPEAL_ALREADY_PENDING", "Khiếu nại của quyền này đang chờ admin xử lý.");

        _context.UserRoleAppeals.Add(new UserRoleAppeal
        {
            RestrictionId = restriction.Id,
            Reason = request.Reason.Trim(),
            SubmittedAt = DateTime.UtcNow,
            Status = RoleAppealStatus.Pending
        });
        await _context.SaveChangesAsync();
        try
        {
            var adminUserIds = await _context.Users
                .Where(account => account.UserRoles.Any(userRole => userRole.Role.Name == "Admin"))
                .Select(account => account.Id)
                .ToListAsync();
            foreach (var adminUserId in adminUserIds)
            {
                await _notificationService.CreateNotificationAsync(adminUserId, new NotificationRequest
                {
                    Title = "Khiếu nại thu hồi quyền mới",
                    Message = $"Người dùng gửi khiếu nại về quyền {restriction.Role.Name}.",
                    Type = (int)NotificationType.SYSTEM,
                    Link = "/admin/role-appeals"
                });
            }
        }
        catch (Exception notificationException)
        {
            Console.Error.WriteLine($"Failed to notify admins about role appeal: {notificationException}");
        }
        return Result.Success();
    }

    public async Task<Result<List<AdminRoleAppealResponse>>> GetPendingAppealsAsync()
    {
        var appeals = await _context.UserRoleAppeals
            .AsNoTracking()
            .Include(appeal => appeal.Restriction)
            .ThenInclude(restriction => restriction.Role)
            .Include(appeal => appeal.Restriction.User)
            .Where(appeal => appeal.Status == RoleAppealStatus.Pending && appeal.Restriction.IsActive)
            .OrderBy(appeal => appeal.SubmittedAt)
            .Select(appeal => new AdminRoleAppealResponse
            {
                AppealId = appeal.Id,
                UserId = appeal.Restriction.UserId,
                UserName = appeal.Restriction.User.FullName,
                UserEmail = appeal.Restriction.User.Email,
                RoleName = appeal.Restriction.Role.Name,
                RestrictionReason = appeal.Restriction.Reason,
                AppealReason = appeal.Reason,
                SubmittedAt = appeal.SubmittedAt
            })
            .ToListAsync();
        return Result<List<AdminRoleAppealResponse>>.Success(appeals);
    }

    public async Task<Result> ReviewAppealAsync(Guid appealId, Guid adminId, ReviewRoleAppealRequest request)
    {
        if (!request.IsApproved && (string.IsNullOrWhiteSpace(request.Response) || request.Response.Trim().Length < 5))
            return HttpErrors.BadRequest("REVIEW_RESPONSE_REQUIRED", "Lý do từ chối khiếu nại phải có ít nhất 5 ký tự.");
        if (request.Response?.Trim().Length > 500)
            return HttpErrors.BadRequest("REVIEW_RESPONSE_TOO_LONG", "Phản hồi không được vượt quá 500 ký tự.");

        var appeal = await _context.UserRoleAppeals
            .Include(item => item.Restriction)
            .ThenInclude(restriction => restriction.Role)
            .FirstOrDefaultAsync(item => item.Id == appealId);
        if (appeal == null)
            return HttpErrors.NotFound("APPEAL_NOT_FOUND", "Không tìm thấy khiếu nại.");
        if (appeal.Status != RoleAppealStatus.Pending || !appeal.Restriction.IsActive)
            return HttpErrors.BadRequest("APPEAL_ALREADY_REVIEWED", "Khiếu nại này đã được xử lý.");

        appeal.Status = request.IsApproved ? RoleAppealStatus.Approved : RoleAppealStatus.Rejected;
        appeal.AdminResponse = request.Response?.Trim();
        appeal.ReviewedByAdminId = adminId;
        appeal.ReviewedAt = DateTime.UtcNow;

        if (request.IsApproved)
        {
            var restriction = appeal.Restriction;
            if (!await _context.UserRoles.AnyAsync(userRole =>
                userRole.UserId == restriction.UserId && userRole.RoleId == restriction.RoleId))
            {
                _context.UserRoles.Add(new UserRole
                {
                    UserId = restriction.UserId,
                    RoleId = restriction.RoleId
                });
            }
            restriction.IsActive = false;

            if (restriction.Role.Name == "Shipper")
            {
                var shipper = await _context.Shippers.FirstOrDefaultAsync(item => item.UserId == restriction.UserId);
                if (shipper != null)
                {
                    shipper.IsActive = true;
                    shipper.IsAvailable = false;
                    shipper.UpdatedAt = DateTime.UtcNow;
                }
            }
        }

        await _context.SaveChangesAsync();
        await _notificationService.CreateNotificationAsync(appeal.Restriction.UserId, new NotificationRequest
        {
            Title = request.IsApproved ? "Khiếu nại đã được chấp thuận" : "Khiếu nại đã bị từ chối",
            Message = request.IsApproved
                ? $"Khiếu nại thu hồi quyền {appeal.Restriction.Role.Name} đã được chấp thuận."
                : $"Khiếu nại thu hồi quyền {appeal.Restriction.Role.Name} đã bị từ chối. Phản hồi: {appeal.AdminResponse}",
            Type = (int)NotificationType.SYSTEM,
            Link = "/account"
        });
        return Result.Success();
    }
}
