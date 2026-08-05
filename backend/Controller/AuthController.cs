using Microsoft.AspNetCore.Mvc;

using FoodDelivery.Service.Interfaces;
using PermissionConstants = FoodDelivery.Constants.Permissions;
using FoodDelivery.DTOs;
using Microsoft.AspNetCore.Authorization;
using FoodDelivery.Extensions;
using FoodDelivery.Common;

namespace FoodDelivery.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private const string RefreshTokenCookieName = "refreshToken";
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

    private void SetRefreshTokenCookie(string token)
    {
        Response.Cookies.Append(RefreshTokenCookieName, token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true, // để false khi test http local nếu cần, nhớ bật lại khi deploy
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7),
            Path = "/api/auth"
        });
    }
    private void DeleteRefreshToken()
    {
        Response.Cookies.Delete(RefreshTokenCookieName, new CookieOptions
        {
            Path = "/api/auth"
        });
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.AuthenticateUserAsync(request.Email, request.Password);
        if (!result.IsSuccess || result.Data is null)
        {
            return result.ToActionResult();
        }
        // RefreshToken -> cookie, không trả trong body
            if (!string.IsNullOrEmpty(result.Data.RefreshToken))
        {
            SetRefreshTokenCookie(result.Data.RefreshToken);
        }
        // Chỉ trả accessToken cho FE
        return Ok(new { accessToken = result.Data.AccessToken });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = User.GetUserId();
        var refreshToken = Request.Cookies[RefreshTokenCookieName];
        if (!string.IsNullOrEmpty(refreshToken))
        {
            await _authService.LogoutAsync(userId, refreshToken);
        }
        DeleteRefreshToken();
        return Ok();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies[RefreshTokenCookieName];
        if (string.IsNullOrEmpty(refreshToken))
        {
            var error = HttpErrors.Unauthorized("REFRESH_MISSING", "Không tìm thấy refresh token");
            return error.ToActionResult();
        }
        var result = await _authService.RefreshAccessTokenAsync(refreshToken);
        if(!result.IsSuccess || result.Data is null)
        {
            DeleteRefreshToken();
            return result.ToActionResult();
        }
        else
        {
            if (!string.IsNullOrEmpty(result.Data.RefreshToken))
            {
                SetRefreshTokenCookie(result.Data.RefreshToken);
            }
        }
        return Ok(new { accessToken = result.Data.AccessToken });
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