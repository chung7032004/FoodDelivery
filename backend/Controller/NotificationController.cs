using FoodDelivery.DTOs.Notification;
using FoodDelivery.Extensions;
using FoodDelivery.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetNotifications([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        var userId = User.GetUserId();
        var result = await _notificationService.GetUserNotificationsAsync(userId, pageNumber, pageSize);
        return result.ToActionResult();
    }

    [HttpGet("unread")]
    public async Task<IActionResult> GetUnreadNotifications()
    {
        var userId = User.GetUserId();
        var result = await _notificationService.GetUnreadNotificationsAsync(userId);
        return result.ToActionResult();
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount()
    {
        var userId = User.GetUserId();
        var result = await _notificationService.GetUnreadCountAsync(userId);    
        return result.ToActionResult();
    }

    [HttpGet("{notificationId}")]
    public async Task<IActionResult> GetNotificationById([FromRoute] Guid notificationId)
    {
        var result = await _notificationService.GetNotificationByIdAsync(notificationId);
        return result.ToActionResult();
    }

    [HttpPost("")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateNotification([FromBody] NotificationRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Message))
            return BadRequest(new { message = "Title and Message are required" });

        // Nếu không có userId trong request, lấy từ token (gửi cho chính mình)
        var userId = User.GetUserId();
        var result = await _notificationService.CreateNotificationAsync(userId, request);

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetNotificationById), new { notificationId = result.Data?.Id }, result);
    }

    [HttpPut("{notificationId}/read")]
    public async Task<IActionResult> MarkAsRead([FromRoute] Guid notificationId)
    {
        var result = await _notificationService.MarkAsReadAsync(notificationId);
        return result.ToActionResult();
    }

    [HttpPut("mark-all-read")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userId = User.GetUserId();
        var result = await _notificationService.MarkAllAsReadAsync(userId);
        return result.ToActionResult();
    }


    [HttpDelete("{notificationId}")]
    public async Task<IActionResult> DeleteNotification([FromRoute] Guid notificationId)
    {
        var result = await _notificationService.DeleteNotificationAsync(notificationId);
        return result.ToActionResult();
    }
}
