using FoodDelivery.Entities;
using FoodDelivery.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Repositories.Implements
{
    public class ShipperRepository : IShipperRepository
    {
        private readonly FoodContext _context;
        public ShipperRepository(FoodContext context) { _context = context; }

        public async Task<OrderDetail?> GetOrderDetailByIdAsync(Guid orderId) => 
            await _context.OrderDetails
                .Include(od => od.Order)
                    .ThenInclude(o => o.OrderItems)
                .Include(od => od.Order)
                    .ThenInclude(o => o.OrderStatusHistories)
                .Include(od => od.Order)
                    .ThenInclude(o => o.Customer)
                .FirstOrDefaultAsync(od => od.OrderId == orderId);

        public void UpdateOrderDetail(OrderDetail orderDetail) => _context.OrderDetails.Update(orderDetail);

        public async Task AddStatusHistoryAsync(OrderStatusHistory history) => await _context.OrderStatusHistories.AddAsync(history);

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;

        public async Task<(List<Shipper> Items, int TotalCount)> GetAllShippersAsync(int page, int pageSize)
        {
            var query = _context.Shippers
                .Where(s => s.User.UserRoles.Any(r => r.Role.Name == "Shipper"))
                .Include(s => s.User)
                .AsNoTracking();
            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(s => s.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (items, totalCount);
        }
        public async Task<(List<Shipper> Items, int TotalCount)> GetPendingApplicationsAsync(int page, int pageSize)
        {
            var query = _context.Shippers
                .Include(s => s.User)
                .Where(s=> s.Status == ShipperStatus.Pending)
                .AsNoTracking();
            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(s => s.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (items, totalCount);
        }
        public async Task<Shipper?> GetShipperByIdAsync(Guid userId)
        {
            return await _context.Shippers
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }
        public async Task<Shipper?> GetShipperByRecordIdAsync(Guid shipperId)
        {
            return await _context.Shippers
                .FirstOrDefaultAsync(s => s.Id == shipperId);
        }
        public async Task<List<OrderDetail>> GetOrdersByShipperAsync(Guid shipperId)
        {
            return await _context.OrderDetails
                .Where(od => od.ShipperId == shipperId)
                .Include(od=> od.Order)
                    .ThenInclude(order => order.Customer)
                .Include(od => od.Order)
                    .ThenInclude(order => order.OrderStatusHistories)
                .ToListAsync();
        }
            
        public async Task<List<OrderStatusHistory>> GetShipperHistoryAsync(Guid userId) => 
            await _context.OrderStatusHistories
                .Include(h => h.Order)
                    .ThenInclude(o => o.Customer)
                .Where(h => h.ChangeByUserId == userId)
                .OrderByDescending(h => h.ChangedAt)
                .ToListAsync();

        public async Task<List<OrderDetail>> GetShipperCompletedOrdersAsync(Guid shipperId) =>
            await _context.OrderDetails
                .Where(od => od.ShipperId == shipperId && od.Status == OrderStatus.Completed)
                .Include(od => od.Order)
                    .ThenInclude(o => o.Customer)
                .OrderByDescending(od => od.Order.CreatedAt)
                .ToListAsync();

        public async Task<List<OrderDetail>> GetAvailableOrdersAsync()
        {
            return await _context.OrderDetails
                .Where(od => od.ShipperId == null
                    && od.DispatchStatus == DispatchStatus.Searching
                    && (od.Status == OrderStatus.Confirmed
                        || od.Status == OrderStatus.Preparing
                        || od.Status == OrderStatus.ReadyForPickup))
                .Include(od => od.Order)
                    .ThenInclude(o => o.Customer)
                .OrderByDescending(od => od.Order.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> TryAssignAvailableOrderAsync(Guid orderId, Guid shipperId, DateTime assignedAt)
        {
            if (_context.Database.CurrentTransaction == null)
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                var assigned = await TryAssignAvailableOrderInTransactionAsync(orderId, shipperId, assignedAt);
                await transaction.CommitAsync();
                return assigned;
            }

            return await TryAssignAvailableOrderInTransactionAsync(orderId, shipperId, assignedAt);
        }

        private async Task<int> TryAssignAvailableOrderInTransactionAsync(
            Guid orderId,
            Guid shipperId,
            DateTime assignedAt)
        {
            // Serialize claims per shipper until the surrounding transaction commits.
            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"SELECT pg_advisory_xact_lock(hashtext({shipperId.ToString()}))");

            var hasActiveAssignment = await _context.OrderDetails.AnyAsync(detail =>
                detail.ShipperId == shipperId
                && detail.Status != OrderStatus.Completed
                && detail.Status != OrderStatus.Cancelled);
            if (hasActiveAssignment)
                return 0;

            return await _context.OrderDetails
                .Where(detail => detail.OrderId == orderId
                    && detail.ShipperId == null
                    && detail.DispatchStatus == DispatchStatus.Searching
                    && !_context.Shippers.Any(shipper =>
                        shipper.Id == shipperId
                        && shipper.UserId == detail.Order.CustomerId)
                    && (detail.Status == OrderStatus.Confirmed
                        || detail.Status == OrderStatus.Preparing
                        || detail.Status == OrderStatus.ReadyForPickup))
                .ExecuteUpdateAsync(updates => updates
                    .SetProperty(detail => detail.ShipperId, shipperId)
                    .SetProperty(detail => detail.DispatchStatus, DispatchStatus.Assigned)
                    .SetProperty(detail => detail.ShipperAssignedAt, assignedAt));
        }

        public async Task<decimal> GetTodayEarningsAsync(Guid shipperId)
        {
            var today = DateTime.UtcNow.Date;
            return await _context.OrderDetails
                .Where(od => od.ShipperId == shipperId 
                    && od.Status == OrderStatus.Completed
                    && od.ActualDeliveryTime >= today
                    && od.ActualDeliveryTime < today.AddDays(1))
                .SumAsync(od => od.Order.ShippingFee);
        }

        public async Task<decimal> GetTotalEarningsAsync(Guid shipperId)
        {
            return await _context.OrderDetails
                .Where(od => od.ShipperId == shipperId && od.Status == OrderStatus.Completed)
                .SumAsync(od => od.Order.ShippingFee);
        }
        public async Task AddShipperAsync(Shipper shipper) => await _context.Shippers.AddAsync(shipper);
        public async Task AddUserRoleAsync(UserRole userRole) => await _context.UserRoles.AddAsync(userRole);
        public async Task<bool> HasShipperRecordAsync(Guid userId)
        {
            return await _context.Shippers.AnyAsync(s => s.UserId == userId);
        }
    }
}