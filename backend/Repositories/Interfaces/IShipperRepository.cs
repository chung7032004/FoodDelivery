using FoodDelivery.Entities;

namespace FoodDelivery.Repositories.Interfaces
{
    public interface IShipperRepository
    {
        Task<OrderDetail?> GetOrderDetailByIdAsync(Guid orderId);
        void UpdateOrderDetail(OrderDetail orderDetail);
        Task AddStatusHistoryAsync(OrderStatusHistory history);
        Task<bool> SaveChangesAsync();
        Task<(List<Shipper> Items, int TotalCount)> GetAllShippersAsync(int page, int pageSize);
        Task<(List<Shipper> Items, int TotalCount)> GetPendingApplicationsAsync(int page, int pageSize);
        Task<Shipper?> GetShipperByIdAsync(Guid userId);
        Task<Shipper?> GetShipperByRecordIdAsync(Guid shipperId);
        Task<List<OrderStatusHistory>> GetShipperHistoryAsync(Guid userId);
        Task<List<OrderDetail>> GetShipperCompletedOrdersAsync(Guid shipperId);
        Task<List<OrderDetail>> GetAvailableOrdersAsync();
        Task AddShipperAsync(Shipper shipper);
        Task<decimal> GetTodayEarningsAsync(Guid shipperId);
        Task<decimal> GetTotalEarningsAsync(Guid shipperId);
        Task AddUserRoleAsync(UserRole userRole);
        Task<List<OrderDetail>> GetOrdersByShipperAsync(Guid shipperId);
        Task<bool> HasShipperRecordAsync(Guid userId);
    }
}
        