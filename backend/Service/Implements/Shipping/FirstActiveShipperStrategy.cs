using FoodDelivery.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace FoodDelivery.Service.Implements.Shipping;
public class FirstActiveShipperStrategy : IShipperAssignmentStrategy
{
    private readonly FoodContext _ctx;
    public FirstActiveShipperStrategy(FoodContext ctx) => _ctx = ctx;

    public async Task<Guid?> FindShipperAsync(Guid orderId, Guid restaurantId)
    {
        // Tối thiểu: chỉ chọn shipper không có đơn Shipping đang giao
        var busyShipperIds = await _ctx.Orders
            .Where(o => o.OrderDetail.Status == OrderStatus.Shipping && o.OrderDetail.ShipperId != null)
            .Select(o => o.OrderDetail.ShipperId!.Value)
            .ToListAsync();

        return await _ctx.Shippers
            .Where(s => s.IsActive && s.User.IsActive && !busyShipperIds.Contains(s.Id))
            .Select(s => (Guid?)s.Id)
            .FirstOrDefaultAsync();
    }
}