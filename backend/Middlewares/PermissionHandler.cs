using System.Security.Claims;
using FoodDelivery.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IPermissionService _permissionService;
    public PermissionHandler(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context , PermissionRequirement requirement)
    {
        // Lấy userId từ JWT claim
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
        {
            return ;
        }
        if (await _permissionService.HasPermissionAsync(userId, requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}