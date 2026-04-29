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
    public AccountController ( IAccountService accountService, IUserService userService)
    {
        _service = accountService;
        _userService = userService;
    }

    [HttpGet("me")]
    [HasPermission(PermissionConstants.Accounts.ViewOwn)]
    public async Task<IActionResult> GetMyAccount()
    {
        var userId = User.GetUserId();
        var result = await _service.GetAccountAsync(userId);
        if (!result.IsSuccess)
        {
            return NotFound(result);
        }
        return Ok(result);
    }

    [HasPermission(PermissionConstants.Accounts.UpdateOwn)]
    [HttpPut("me")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateAccount([FromForm] AccountRequest accountRequest)
    {
        var userId = User.GetUserId();
        var result = await _service.UpdateAccountAsync(userId, accountRequest);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpGet]
    [HasPermission(PermissionConstants.Accounts.ViewAll)]
    public async Task<IActionResult> GetAllUsers([FromQuery] UserFilterModel filter)
    {
        var result = await _userService.GetAllUsersAsync(filter);
        return Ok(result);
    }

    [HttpGet("{id}")]
    [HasPermission(PermissionConstants.Accounts.View)]
    public async Task<IActionResult> GetUserDetail(Guid id)
    {
        var result = await _userService.GetUserDetailAsync(id);
        if (!result.IsSuccess)
        {
            return NotFound(result);
        }
        return Ok(result);
    }

    [HttpPatch("{id}/block")]
    [HasPermission(PermissionConstants.Accounts.Lock)]
    public async Task<IActionResult> BlockUser(Guid id)
    {
        var result = await _userService.BlockUserAsync(id);
        if (!result.IsSuccess)
        {
            return NotFound(result);
        }
        return Ok(result);
    }

    [HttpPatch("{id}/unblock")]
    [HasPermission(PermissionConstants.Accounts.Unlock)]
    public async Task<IActionResult> UnblockUser(Guid id)
    {
        var result = await _userService.UnblockUserAsync(id);
        if (!result.IsSuccess)
        {
            return NotFound(result);
        }
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    [HasPermission(PermissionConstants.Accounts.Edit)]
    public async Task<IActionResult> UpdateUser(Guid id, [FromForm] AccountRequest accountRequest)
    {
        var result = await _service.UpdateAccountAsync(id, accountRequest);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [HasPermission(PermissionConstants.Accounts.Delete)]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var result = await _service.DeleteAccountAsync(id);
        if (!result.IsSuccess)
        {
            return NotFound(result);
        }
        return Ok();
    }
    
}
