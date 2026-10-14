using FoodDelivery.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

[Authorize]
public class NotificationHub : Hub
{
    public static string GetUserGroup(Guid userId) => $"user:{userId}";

    public override async Task OnConnectedAsync()
    {
        if (Context.User == null || !Context.User.TryGetUserId(out var userId))
        {
            Context.Abort();
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, GetUserGroup(userId));
        await base.OnConnectedAsync();
    }
}
