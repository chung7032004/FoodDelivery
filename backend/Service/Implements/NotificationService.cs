using FoodDelivery.Common;
using FoodDelivery.DTOs.Notification;
using FoodDelivery.Entities;
using FoodDelivery.Repositories.Interfaces;
using FoodDelivery.Service.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace FoodDelivery.Service.Implements;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly FoodContext _context;
    private readonly IHubContext<NotificationHub> _notificationHub;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        INotificationRepository notificationRepository,
        FoodContext context,
        IHubContext<NotificationHub> notificationHub,
        ILogger<NotificationService> logger)
    {
        _notificationRepository = notificationRepository;
        _context = context;
        _notificationHub = notificationHub;
        _logger = logger;
    }

    public async Task<Result<NotificationResponse>> CreateNotificationAsync(Guid userId, NotificationRequest request)
    {
        try
        {
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = request.Title.Trim(),
                Message = request.Message.Trim(),
                Type = (NotificationType)request.Type,
                Link = request.Link?.Trim(),
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationRepository.CreateAsync(notification);
            await _context.SaveChangesAsync();

            var response = MapToResponse(notification);
            try
            {
                await _notificationHub.Clients
                    .Group(NotificationHub.GetUserGroup(userId))
                    .SendAsync("ReceiveNotification", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send notification {NotificationId} to user {UserId} in real time", notification.Id, userId);
            }

            return Result<NotificationResponse>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create notification for user {UserId}", userId);
            return Result<NotificationResponse>.Failure("CREATE_NOTIFICATION_ERROR", $"Error creating notification: {ex.Message}");
        }
    }

    public async Task<Result<NotificationSummaryResponse>> GetUserNotificationsAsync(Guid userId, int pageNumber = 1, int pageSize = 20)
    {
        try
        {
            var notifications = await _notificationRepository.GetUserNotificationsAsync(userId, pageNumber, pageSize);
            var unreadCount = await _notificationRepository.GetUnreadCountAsync(userId);

            var response = new NotificationSummaryResponse
            {
                UnreadCount = unreadCount,
                Notifications = notifications.Select(MapToResponse).ToList()
            };

            return Result<NotificationSummaryResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<NotificationSummaryResponse>.Failure("GET_NOTIFICATIONS_ERROR", $"Error fetching notifications: {ex.Message}");
        }
    }

    public async Task<Result<List<NotificationResponse>>> GetUnreadNotificationsAsync(Guid userId)
    {
        try
        {
            var notifications = await _notificationRepository.GetUnreadNotificationsAsync(userId);
            return Result<List<NotificationResponse>>.Success(
                notifications.Select(MapToResponse).ToList()
            );
        }
        catch (Exception ex)
        {
            return Result<List<NotificationResponse>>.Failure("GET_UNREAD_ERROR", $"Error fetching unread notifications: {ex.Message}");
        }
    }

    public async Task<Result<NotificationResponse>> GetNotificationByIdAsync(Guid userId, Guid notificationId)
    {
        try
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null || notification.UserId != userId)
                return Result<NotificationResponse>.Failure("NOT_FOUND", "Notification not found");

            return Result<NotificationResponse>.Success(MapToResponse(notification));
        }
        catch (Exception ex)
        {
            return Result<NotificationResponse>.Failure("GET_NOTIFICATION_ERROR", $"Error fetching notification: {ex.Message}");
        }
    }

    public async Task<Result<int>> GetUnreadCountAsync(Guid userId)
    {
        try
        {
            var count = await _notificationRepository.GetUnreadCountAsync(userId);
            return Result<int>.Success(count);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure("GET_COUNT_ERROR", $"Error fetching unread count: {ex.Message}");
        }
    }

    public async Task<Result<NotificationResponse>> MarkAsReadAsync(Guid userId, Guid notificationId)
    {
        try
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null || notification.UserId != userId)
                return Result<NotificationResponse>.Failure("NOT_FOUND", "Notification not found");

            notification.IsRead = true;
            await _notificationRepository.UpdateAsync(notification);
            await _context.SaveChangesAsync();

            return Result<NotificationResponse>.Success(MapToResponse(notification));
        }
        catch (Exception ex)
        {
            return Result<NotificationResponse>.Failure("UPDATE_ERROR", $"Error updating notification: {ex.Message}");
        }
    }

    public async Task<Result<int>> MarkAllAsReadAsync(Guid userId)
    {
        try
        {
            var count = await _notificationRepository.MarkAllAsReadAsync(userId);
            await _context.SaveChangesAsync();

            return Result<int>.Success(count);
        }
        catch (Exception ex)
        {
            return Result<int>.Failure("MARK_ALL_ERROR", $"Error marking notifications as read: {ex.Message}");
        }
    }

    public async Task<Result<bool>> DeleteNotificationAsync(Guid userId, Guid notificationId)
    {
        try
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            if (notification == null || notification.UserId != userId)
                return Result<bool>.Failure("NOT_FOUND", "Notification not found");

            await _notificationRepository.DeleteAsync(notificationId);
            await _context.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure("DELETE_ERROR", $"Error deleting notification: {ex.Message}");
        }
    }

    private NotificationResponse MapToResponse(Notification notification)
    {
        return new NotificationResponse
        {
            Id = notification.Id,
            Title = notification.Title,
            Message = notification.Message,
            Type = notification.Type.ToString(),
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt,
            Link = notification.Link
        };
    }
}
