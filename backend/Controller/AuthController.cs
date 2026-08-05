using Microsoft.AspNetCore.Mvc;

using FoodDelivery.Service.Interfaces;
using PermissionConstants = FoodDelivery.Constants.Permissions;
using FoodDelivery.DTOs;
using Microsoft.AspNetCore.Authorization;
using FoodDelivery.Extensions;

namespace FoodDelivery.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register ([FromBody] RegisterRequest request)
    {
        var result = await _authService.RegisterUserAsync(request.Email, request.Password, request.FullName, request.Phone);
        return result.ToActionResult();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.AuthenticateUserAsync(request.Email, request.Password);
        return result.ToActionResult();
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest logoutRequest)
    {
        var userId = User.GetUserId();
        var result = await _authService.LogoutAsync(userId,logoutRequest.RefreshToken);
        return result.ToActionResult();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken ([FromBody] RefreshTokenRequest refreshTokenRequest)
    {
        var result = await _authService.RefreshAccessTokenAsync(refreshTokenRequest.RefreshToken);
        return result.ToActionResult();
    }
    [Authorize]
    [HttpPost("change-password")]
    [HasPermission(PermissionConstants.Accounts.ChangePassword)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest)
    {
        var userId = User.GetUserId();
        var result = await _authService.ChangePasswordAsync(userId,changePasswordRequest);
        return result.ToActionResult();
    }
    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest request)
    {
        var result = await _authService.SendOtpAsync(request);
        return result.ToActionResult();
    }
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _authService.ResetPasswordAsync(
            request.Email, 
            request.Otp, 
            request.NewPassword
        );
        return result.ToActionResult();
    }
}