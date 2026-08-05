using FoodDelivery.Common;
using FoodDelivery.DTOs;
using FoodDelivery.DTOs.Shipper;
using FoodDelivery.Entities;

namespace FoodDelivery.Service.Interfaces
{
    public interface IShipperService
    {
        Task<Result<ShipperStatsDto>> GetShipperStatsAsync(Guid userId);
        Task<Result<string>> ApplyShipperAsync(Guid userId, ApplyShipperDto dto);
        Task<Result<string>> ReviewApplicationAsync(Guid shipperId, Guid adminId, ReviewShipperDto dto);
        Task<Result<PagedResponse<ShipperApplicationDto>>> GetAllShippersAsync(int page, int pageSize);
        Task<Result<PagedResponse<ShipperApplicationDto>>> GetPendingApplicationsAsync(int page, int pageSize);


        // --- 3 chức năng nghiệp vụ giao hàng ---
        Task<bool> ConfirmPickUpAsync(Guid orderId, Guid userId);
        Task<bool> MarkSuccessAsync(Guid orderId, Guid userId);
        Task<bool> MarkFailedAsync(Guid orderId, string reason, Guid? cancelledBy = null);

        
        
        // --- 5 chức năng quản lý 
   
        Task<Shipper?> GetShipperByIdAsync(Guid userId); // Chức năng 3
        Task<bool> ToggleShipperStatusAsync(Guid userId, bool isActive); // Chức năng 4
        Task<List<OrderStatusHistory>> GetShipperHistoryAsync(Guid userId); // Chức năng 5
        Task<List<OrderAdminSummaryResponse>> GetShipperCompletedOrdersAsync(Guid userId); // Lấy danh sách đơn hoàn thành
        
        Task<List<OrderAdminSummaryResponse>> GetAssignedOrdersAsync(Guid userId);
        Task<List<OrderAdminSummaryResponse>> GetAvailableOrdersAsync(); // Lấy danh sách orders sẵn sàng để nhận
        Task<bool> AcceptOrderAsync(Guid orderId, Guid shipperId); // Shipper chấp nhận/nhận đơn
        Task<OrderDetailResponse?> GetOrderByIdAsync(Guid orderId);
        Task<bool> UpdateShipperProfileAsync(Guid userId, UpdateShipperProfileDto request);
    }
}