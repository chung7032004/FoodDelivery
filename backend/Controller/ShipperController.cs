using FoodDelivery.DTOs.Shipper;
using FoodDelivery.Extensions;
using FoodDelivery.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PermissionConstants = FoodDelivery.Constants.Permissions;

namespace FoodDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShipperController : ControllerBase
    {
        private readonly IShipperService _shipperService;

        public ShipperController(IShipperService service)
        {
            _shipperService = service;
        }

        [HttpGet("application/me")]
        [HasPermission(PermissionConstants.Shippers.ApplyAsShipper)]
        [ProducesResponseType(typeof(FoodDelivery.Common.Result<MyShipperApplicationDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMyApplication()
        {
            var userId = User.GetUserId();
            var result = await _shipperService.GetMyApplicationAsync(userId);
            return result.ToActionResult();
        }

        [HttpPost("apply")]
        [HasPermission(PermissionConstants.Shippers.ApplyAsShipper)]
        public async Task<IActionResult> Apply([FromBody] ApplyShipperDto dto)
        {
            var userId = User.GetUserId();
            var result = await _shipperService.ApplyShipperAsync(userId, dto);
            return result.ToActionResult();
        }

        [HttpGet("applications/pending")]
        [HasPermission(PermissionConstants.Shippers.ViewApplications)]
        public async Task<IActionResult> GetPendingApplications([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _shipperService.GetPendingApplicationsAsync(page, pageSize);
            return result.ToActionResult();
        }

        [HttpGet("all")]
        [HasPermission(PermissionConstants.Shippers.ViewAll)]
        public async Task<IActionResult> GetAllShippers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _shipperService.GetAllShippersAsync(page, pageSize);
            return result.ToActionResult();
        }

        [HttpPut("applications/{shipperId}/review")]
        [HasPermission(PermissionConstants.Shippers.ReviewApplications)]
        public async Task<IActionResult> ReviewApplication(Guid shipperId, [FromBody] ReviewShipperDto dto)
        {
            var adminId = User.GetUserId();
            var result = await _shipperService.ReviewApplicationAsync(shipperId, adminId, dto);
            return result.ToActionResult();
        }

        [HttpGet("dashboard")]
        [HasPermission(PermissionConstants.Shippers.ViewDashboard)]
        public async Task<IActionResult> GetDashboardStats()
        {
            var userId = User.GetUserId();
            var result = await _shipperService.GetShipperStatsAsync(userId);
            return result.ToActionResult();
        }

        [HttpGet("orders/assigned")]
        [HasPermission(PermissionConstants.Shippers.ViewAssignedOrders)]
        public async Task<IActionResult> GetAssignedOrders()
        {
            var userId = User.GetUserId();
            var data = await _shipperService.GetAssignedOrdersAsync(userId);
            return Ok(data);
        }

        [HttpGet("orders/available")]
        [HasPermission(PermissionConstants.Shippers.AcceptOrder)]
        public async Task<IActionResult> GetAvailableOrders()
        {
            var data = await _shipperService.GetAvailableOrdersAsync();
            return Ok(data);
        }

        [HttpGet("orders/completed")]
        [HasPermission(PermissionConstants.Shippers.ViewDeliveryHistory)]
        public async Task<IActionResult> GetCompletedOrders()
        {
            var userId = User.GetUserId();
            var data = await _shipperService.GetShipperCompletedOrdersAsync(userId);
            return Ok(data);
        }

        [HttpGet("orders/{orderId}")]
        [HasPermission(PermissionConstants.Shippers.ViewOrderDetails)]
        public async Task<IActionResult> GetOrderDetail(Guid orderId)
        {
            var data = await _shipperService.GetOrderByIdAsync(orderId);
            return data == null ? NotFound(new { message = "Không tìm thấy đơn hàng" }) : Ok(data);
        }

        [HttpPost("orders/{orderId}/accept")]
        [HasPermission(PermissionConstants.Shippers.AcceptOrder)]
        public async Task<IActionResult> AcceptOrder(Guid orderId)
        {
            var userId = User.GetUserId();
            var success = await _shipperService.AcceptOrderAsync(orderId, userId);
            return success
                ? Ok(new { message = "Đã nhận đơn hàng thành công" })
                : BadRequest(new { message = "Không thể nhận đơn hàng tại thời điểm này" });
        }

        [HttpPost("orders/{orderId}/pickup")]
        [HasPermission(PermissionConstants.Shippers.ConfirmPickup)]
        public async Task<IActionResult> ConfirmPickup(Guid orderId)
        {
            var userId = User.GetUserId();
            var success = await _shipperService.ConfirmPickUpAsync(orderId, userId);
            return success
                ? Ok(new { message = "Đã xác nhận lấy hàng thành công" })
                : BadRequest(new { message = "Không thể xác nhận lấy hàng" });
        }

        [HttpPost("orders/{orderId}/complete")]
        [HasPermission(PermissionConstants.Shippers.MarkDeliverySuccess)]
        public async Task<IActionResult> MarkSuccess(Guid orderId)
        {
            var userId = User.GetUserId();
            var success = await _shipperService.MarkSuccessAsync(orderId, userId);
            return success
                ? Ok(new { message = "Đã giao hàng thành công" })
                : BadRequest(new { message = "Không thể cập nhật trạng thái giao hàng" });
        }

        [HttpPost("orders/{orderId}/fail")]
        [HasPermission(PermissionConstants.Shippers.CancelAssignedOrder)]
        public async Task<IActionResult> MarkFailed(Guid orderId, [FromBody] ShipperActionDTO request)
        {
            var userId = User.GetUserId();
            var success = await _shipperService.MarkFailedAsync(orderId, request.Reason ?? "Giao hàng thất bại", userId);
            return success
                ? Ok(new { message = "Đã ghi nhận giao hàng thất bại" })
                : BadRequest(new { message = "Không thể cập nhật trạng thái giao hàng" });
        }

        [HttpGet("history")]
        [HasPermission(PermissionConstants.Shippers.ViewDeliveryHistory)]
        public async Task<IActionResult> GetHistory()
        {
            var userId = User.GetUserId();
            var data = await _shipperService.GetShipperHistoryAsync(userId);
            return Ok(data);
        }

        [HttpGet("profile")]
        [HasPermission(PermissionConstants.Shippers.ViewDashboardStats)]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.GetUserId();
            var shipper = await _shipperService.GetShipperByIdAsync(userId);
            if (shipper == null)
                return NotFound(new { message = "Không tìm thấy hồ sơ shipper" });

            return Ok(new
            {
                shipperId = shipper.Id,
                userId = shipper.UserId,
                status = shipper.Status,
                isActive = shipper.IsActive,
                isAvailable = shipper.IsAvailable,
                totalDeliveredOrders = shipper.TotalDeliveredOrders,
                updatedAt = shipper.UpdatedAt
            });
        }

        [HttpPut("profile")]
        [HasPermission(PermissionConstants.Shippers.UpdateAvailability)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateShipperProfileDto request)
        {
            var userId = User.GetUserId();
            var success = await _shipperService.UpdateShipperProfileAsync(userId, request);
            return success
                ? Ok(new { message = "Cập nhật trạng thái shipper thành công" })
                : NotFound(new { message = "Không tìm thấy hồ sơ shipper" });
        }
    }
}