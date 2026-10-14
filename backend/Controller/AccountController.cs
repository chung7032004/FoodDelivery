using PermissionConstants = FoodDelivery.Constants.Permissions;
using FoodDelivery.DTOs.Account;
using FoodDelivery.Extensions;
using FoodDelivery.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FoodDelivery.DTOs.User;

namespace FoodDelivery.Controllers;

[ApiController]
[Route("api/accounts")]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IAccountService _service;
    private readonly IUserService _userService;
    private readonly IUserRoleAccessService _roleAccessService;
    public AccountController ( IAccountService accountService, IUserService userService, IUserRoleAccessService roleAccessService)
    {
        _service = accountService;
        _userService = userService;
        _roleAccessService = roleAccessService;
    }

    [HttpGet("me")]
    [HasPermission(PermissionConstants.Accounts.ViewOwn)]
    public async Task<IActionResult> GetMyAccount()
    {
        var userId = User.GetUserId();
        var result = await _service.GetAccountAsync(userId);
        return result.ToActionResult();
    }

    [HasPermission(PermissionConstants.Accounts.UpdateOwn)]
    [HttpPut("me")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateAccount([FromForm] AccountRequest accountRequest)
    {
        var userId = User.GetUserId();
        var result = await _service.UpdateAccountAsync(userId, accountRequest);
        return result.ToActionResult();
    }

    [HttpGet]
    [HasPermission(PermissionConstants.Accounts.ViewAll)]
    public async Task<IActionResult> GetAllUsers([FromQuery] UserFilterModel filter)
    {
        var result = await _userService.GetAllUsersAsync(filter);
        return result.ToActionResult();
    }

    [HttpGet("{id}")]
    [HasPermission(PermissionConstants.Accounts.View)]
    public async Task<IActionResult> GetUserDetail(Guid id)
    {
        var result = await _userService.GetUserDetailAsync(id);
        return result.ToActionResult();
    }

    [HttpPatch("{id}/block")]
    [HasPermission(PermissionConstants.Accounts.Lock)]
    public async Task<IActionResult> BlockUser(Guid id)
    {
        var result = await _userService.BlockUserAsync(id);
        return result.ToActionResult();
    }

    [HttpPatch("{id}/unblock")]
    [HasPermission(PermissionConstants.Accounts.Unlock)]
    public async Task<IActionResult> UnblockUser(Guid id)
    {
        var result = await _userService.UnblockUserAsync(id);
        return result.ToActionResult();
    }

    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    [HasPermission(PermissionConstants.Accounts.Edit)]
    public async Task<IActionResult> UpdateUser(Guid id, [FromForm] AccountRequest accountRequest)
    {
        var result = await _service.UpdateAccountAsync(id, accountRequest);
        return result.ToActionResult();
    }

    [HttpDelete("{id}")]
    [HasPermission(PermissionConstants.Accounts.Delete)]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var result = await _service.DeleteAccountAsync(id);
        return result.ToActionResult();
    }

    [HttpPost("{id}/roles/{roleName}/restriction")]
    [HasPermission(PermissionConstants.Accounts.Lock)]
    public async Task<IActionResult> SuspendRole(Guid id, string roleName, [FromBody] SuspendUserRoleRequest request)
    {
        var result = await _roleAccessService.SuspendRoleAsync(id, User.GetUserId(), roleName, request);
        return result.ToActionResult();
    }

    [HttpGet("me/role-restrictions")]
    public async Task<IActionResult> GetMyRoleRestrictions()
    {
        var result = await _roleAccessService.GetMyRestrictionsAsync(User.GetUserId());
        return result.ToActionResult();
    }

    [HttpPost("me/role-appeals")]
    public async Task<IActionResult> SubmitRoleAppeal([FromBody] SubmitRoleAppealRequest request)
    {
        var result = await _roleAccessService.SubmitAppealAsync(User.GetUserId(), request);
        return result.ToActionResult();
    }

    [HttpGet("role-appeals")]
    [HasPermission(PermissionConstants.Accounts.ViewAll)]
    public async Task<IActionResult> GetPendingRoleAppeals()
    {
        var result = await _roleAccessService.GetPendingAppealsAsync();
        return result.ToActionResult();
    }

    [HttpPatch("role-appeals/{appealId}/review")]
    [HasPermission(PermissionConstants.Accounts.Unlock)]
    public async Task<IActionResult> ReviewRoleAppeal(Guid appealId, [FromBody] ReviewRoleAppealRequest request)
    {
        var result = await _roleAccessService.ReviewAppealAsync(appealId, User.GetUserId(), request);
        return result.ToActionResult();
    }
    
}
