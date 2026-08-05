using FoodDelivery.Entities;
using FoodDelivery.Service.Interfaces;
using FoodDelivery.Repositories.Interfaces;
using FoodDelivery.Common;
using FoodDelivery.DTOs.Shipper;
using FoodDelivery.Repositories;
using FoodDelivery.Constants;

namespace FoodDelivery.Service.Implements
{
    public class ShipperService : IShipperService
    {

        private readonly IShipperRepository _shipperRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly FoodContext _context;

        public ShipperService(IShipperRepository shipperRepository, IRoleRepository roleRepository, IUserRepository userRepository, FoodContext context)
        {
            _shipperRepository = shipperRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _context = context;
        }
        public async Task<Result<PagedResponse<ShipperApplicationDto>>> GetAllShippersAsync(int page, int pageSize)
        {
            pageSize = pageSize > 100 ? 100 : pageSize < 1 ? 10 : pageSize;
            page = page < 1 ? 1 : page;
            var (items, totalCount) = await _shipperRepository.GetAllShippersAsync(page, pageSize);
            var data = items.Select(s=> new ShipperApplicationDto
            {
                ShipperId = s.Id,
                UserId = s.UserId,
                FullName = s.User.FullName, 
                Email = s.User.Email, 
                Phone = s.User.Phone,
                IdentityCard = s.IdentityCard,
                VehicleType = s.VehicleType,
                VehiclePlate = s.VehiclePlate,
                Status = s.Status,
                AppliedAt = s.CreatedAt,
            }).ToList();
            var responseMeta = new PaginationMeta(
                Page:page,
                PageSize : items.Count,
                TotalCount : totalCount,
                TotalPages : (int)Math.Ceiling((double)totalCount/pageSize)
            );
            var response = new PagedResponse<ShipperApplicationDto>(
                Meta : responseMeta,
                Data : data
            );
            return Result<PagedResponse<ShipperApplicationDto>>.Success(response);
        }
        public async Task<Result<ShipperStatsDto>> GetShipperStatsAsync(Guid userId)
        {
            var shipper = await _shipperRepository.GetShipperByIdAsync(userId);
            if (shipper == null) 
                return HttpErrors.NotFound<ShipperStatsDto>("SHIPPER_NOTFOUND","Shipper không tồn tại");
            
            var orders = await _shipperRepository.GetOrdersByShipperAsync(shipper.Id);
            var completeCount = orders.Count(o => o.Status == OrderStatus.Completed);
            var failedCount = orders.Count(o => o.Status == OrderStatus.Cancelled);
            var totalEarnings = orders.Where(o => o.Status == OrderStatus.Completed).Sum(o => o.Order?.TotalAmount ?? 0);
            
            var result = new ShipperStatsDto
            {
                TotalAssignedOrders = orders.Count(),
                TotalCompletedOrders = completeCount,
                TotalFailedOrders = failedCount,
                TotalEarnings = totalEarnings,
                AverageRating = 0.0
            };
            return Result<ShipperStatsDto>.Success(result);
        }
        public async Task<Result<string>> ApplyShipperAsync(Guid userId, ApplyShipperDto dto)
        {
            var existing = await _shipperRepository.HasShipperRecordAsync(userId);
            if (existing)
                return HttpErrors.BadRequest<string>("ALREADY_APPLIED", "Bạn đã đăng ký làm shipper rồi");

            var shipper = new Shipper
            {
                UserId       = userId,
                IdentityCard = dto.IdentityCard,
                VehicleType  = dto.VehicleType,
                VehiclePlate = dto.VehiclePlate,
                Status       = ShipperStatus.Pending,
                IsActive     = false,
                IsAvailable  = false
            };

            await _shipperRepository.AddShipperAsync(shipper);
            await _context.SaveChangesAsync();

            return Result<string>.Success("Đăng ký thành công, vui lòng chờ admin duyệt");
        }
        public async Task<Result<string>> ReviewApplicationAsync(Guid shipperId, Guid adminId, ReviewShipperDto dto)
        {
            var shipper = await _shipperRepository.GetShipperByIdAsync(shipperId);
            if (shipper == null)
                return HttpErrors.NotFound<string>("SHIPPER_NOTFOUND", "Không tìm thấy đơn đăng ký");

            if (shipper.Status != ShipperStatus.Pending)
                return HttpErrors.BadRequest<string>("ALREADY_REVIEWED", "Đơn này đã được xử lý rồi");

            if (dto.IsApproved)
            {
                shipper.Status      = ShipperStatus.Approved;
                shipper.IsActive    = true;
                shipper.IsAvailable = true;

                // Gán role Shipper cho User
                var user = await _userRepository.GetUserByIdWithRoleAsync(shipper.UserId);
                if (user == null) 
                    return HttpErrors.NotFound<string>("USER_NOTFOUND", "User không tồn tại");
                if (user.UserRoles.Any(ur => ur.RoleId == RoleIds.Shipper)) 
                    return HttpErrors.BadRequest<string>("USER_ALREADY_SHIPPER", "User đã là shipper");
                var role = await _roleRepository.GetByIdAsync(RoleIds.Shipper);
                if (role == null) 
                    return HttpErrors.NotFound<string>("ROLE_NOTFOUND", "Role 'Shipper' không tồn tại");
                await _shipperRepository.AddUserRoleAsync(new UserRole { UserId = shipper.UserId, RoleId = role.Id });
            }
            else
            {
                shipper.Status       = ShipperStatus.Rejected;
                shipper.RejectReason = dto.RejectReason;
            }

            shipper.ReviewedAt        = DateTime.UtcNow;
            shipper.ReviewedByAdminId = adminId;
            shipper.UpdatedAt         = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Result<string>.Success(dto.IsApproved ? "Đã duyệt shipper" : "Đã từ chối đơn");
        }

        public async Task<Result<PagedResponse<ShipperApplicationDto>>> GetPendingApplicationsAsync(int page, int pageSize)
        {
            pageSize = pageSize > 100 ? 100 : pageSize < 1 ? 10 : pageSize;
            page = page < 1 ? 1 : page;
            var (items, totalCount) = await _shipperRepository.GetPendingApplicationsAsync(page, pageSize);
            var data = items.Select(s=> new ShipperApplicationDto
            {
                ShipperId = s.Id,
                UserId = s.UserId,
                FullName = s.User.FullName, 
                Email = s.User.Email, 
                Phone = s.User.Phone,
                IdentityCard = s.IdentityCard,
                VehicleType = s.VehicleType,
                VehiclePlate = s.VehiclePlate,
                Status = s.Status,
                AppliedAt = s.CreatedAt,
            }).ToList();
            var responseMeta = new PaginationMeta(
                Page:page,
                PageSize : items.Count,
                TotalCount : totalCount,
                TotalPages : (int)Math.Ceiling((double)totalCount/pageSize)
            );
            var response = new PagedResponse<ShipperApplicationDto>(
                Meta : responseMeta,
                Data : data
            );
            return Result<PagedResponse<ShipperApplicationDto>>.Success(response);
        }













        public async Task<bool> ConfirmPickUpAsync(Guid orderId, Guid userId)
        {
            var orderDetail = await _shipperRepository.GetOrderDetailByIdAsync(orderId);
            if (orderDetail == null || orderDetail.Status != OrderStatus.ReadyForPickup) return false;

            var shipper = await _shipperRepository.GetShipperByIdAsync(userId);
            if (shipper == null || shipper.Status != ShipperStatus.Approved || !shipper.IsActive || !shipper.IsAvailable) return false;
            if (orderDetail.ShipperId != shipper.Id) return false;

            orderDetail.Status = OrderStatus.Shipping;
            
            _shipperRepository.UpdateOrderDetail(orderDetail);
            await _shipperRepository.AddStatusHistoryAsync(new OrderStatusHistory { 
                OrderId = orderId, Status = OrderStatus.Shipping, ChangeByUserId = userId, ChangedAt = DateTime.UtcNow, ActionBy = OrderActionBy.Shipper
            });
            return await _shipperRepository.SaveChangesAsync();
        }
       public async Task<List<OrderAdminSummaryResponse>> GetAssignedOrdersAsync(Guid userId)
        {
            var shipper = await _shipperRepository.GetShipperByIdAsync(userId);
            if (shipper?.Orders == null || shipper.Orders.Count == 0) 
                return new List<OrderAdminSummaryResponse>();

            return shipper.Orders
                .Where(o => o.Status == OrderStatus.ReadyForPickup || o.Status == OrderStatus.Shipping)
                .OrderByDescending(o => o.Order?.CreatedAt)
                .Select(o => new OrderAdminSummaryResponse
                {
                    Id = o.OrderId,
                    OrderCode = o.Order?.OrderCode ?? o.OrderId.ToString().Substring(0, 8),
                    CustomerName = o.Order?.Customer?.FullName ?? "Khách hàng",
                    TotalAmount = o.Order?.TotalAmount ?? 0,
                    Status = o.Status.ToString(),
                    CreatedAt = o.Order?.CreatedAt ?? DateTime.UtcNow
                }).ToList();
        }

        public async Task<List<OrderAdminSummaryResponse>> GetAvailableOrdersAsync()
        {
            var availableOrders = await _shipperRepository.GetAvailableOrdersAsync();
            return availableOrders
                .Select(o => new OrderAdminSummaryResponse
                {
                    Id = o.OrderId,
                    OrderCode = o.Order?.OrderCode ?? o.OrderId.ToString().Substring(0, 8),
                    CustomerName = o.Order?.Customer?.FullName ?? "Khách hàng",
                    TotalAmount = o.Order?.TotalAmount ?? 0,
                    Status = o.Status.ToString(),
                    CreatedAt = o.Order?.CreatedAt ?? DateTime.UtcNow
                }).ToList();
        }

        public async Task<List<OrderAdminSummaryResponse>> GetShipperCompletedOrdersAsync(Guid userId)
        {
            var shipper = await _shipperRepository.GetShipperByIdAsync(userId);
            if (shipper == null)
                return new List<OrderAdminSummaryResponse>();

            var completedOrders = await _shipperRepository.GetShipperCompletedOrdersAsync(shipper.Id);
            return completedOrders
                .Select(o => new OrderAdminSummaryResponse
                {
                    Id = o.OrderId,
                    OrderCode = o.Order?.OrderCode ?? o.OrderId.ToString().Substring(0, 8),
                    CustomerName = o.Order?.Customer?.FullName ?? "Khách hàng",
                    TotalAmount = o.Order?.TotalAmount ?? 0,
                    Status = o.Status.ToString(),
                    CreatedAt = o.Order?.CreatedAt ?? DateTime.UtcNow
                }).ToList();
        }

        public async Task<bool> AcceptOrderAsync(Guid orderId, Guid shipperId)
        {
            var orderDetail = await _shipperRepository.GetOrderDetailByIdAsync(orderId);
            if (orderDetail == null)
                return false;

            if (orderDetail.Status != OrderStatus.ReadyForPickup)
                return false;

            if (orderDetail.ShipperId != null && orderDetail.ShipperId != Guid.Empty)
                return false;

            var shipper = await _shipperRepository.GetShipperByIdAsync(shipperId);
            if (shipper == null || shipper.Status != ShipperStatus.Approved || !shipper.IsActive || !shipper.IsAvailable)
                return false;
            
            await _shipperRepository.AddStatusHistoryAsync(new OrderStatusHistory 
            { 
                OrderId = orderId, 
                Status = OrderStatus.ReadyForPickup, 
                ChangeByUserId = shipperId, 
                ChangedAt = DateTime.UtcNow, 
                ActionBy = OrderActionBy.Shipper,
                Note = "Shipper đã nhận đơn hàng"
            });
            
            return await _shipperRepository.SaveChangesAsync();
        }
        
        public async Task<OrderDetailResponse?> GetOrderByIdAsync(Guid orderId)
        {
            var orderDetail = await _shipperRepository.GetOrderDetailByIdAsync(orderId);
            if (orderDetail?.Order == null) return null;

            return new OrderDetailResponse
            {
                OrderId = orderDetail.OrderId,
                OrderCode = orderDetail.Order.OrderCode,
                CreatedAt = orderDetail.Order.CreatedAt,
                CustomerId = orderDetail.Order.CustomerId,
                CustomerName = orderDetail.Order.Customer?.FullName ?? "Khách hàng",
                CustomerEmail = orderDetail.Order.Customer?.Email ?? string.Empty,
                ShippingAddress = orderDetail.Order.ShippingAddress,
                ReceiverName = orderDetail.Order.ReceiverName,
                ReceiverPhone = orderDetail.Order.ReceiverPhone,
                ShippingFee = orderDetail.Order.ShippingFee,
                EstimatedDeliveryTime = orderDetail.EstimatedDeliveryTime,
                ActualDeliveryTime = orderDetail.ActualDeliveryTime,
                PaymentMethod = orderDetail.PaymentMethod.ToString(),
                PaymentStatus = orderDetail.PaymentStatus.ToString(),
                CurrentStatus = orderDetail.Status.ToString(),
                CancelReason = orderDetail.CancelReason,
                TotalAmount = orderDetail.Order.TotalAmount,
                ShipperId = orderDetail.ShipperId,
                Items = orderDetail.Order.OrderItems?.Select(i => new OrderItemDTO
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    ProductImage = i.ProductImage,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.UnitPrice * i.Quantity,
                    IsRemoved = i.IsRemoved,
                    RemoveReason = i.RemoveReason
                }).ToList() ?? new List<OrderItemDTO>(),
                StatusHistories = orderDetail.Order.OrderStatusHistories?.Select(h => new OrderStatusHistoryDTO
                {
                    Status = h.Status.ToString(),
                    ChangedAt = h.ChangedAt,
                    ChangedBy = h.ChangeByUserId,
                    Note = h.Note
                }).OrderByDescending(h => h.ChangedAt).ToList() ?? new List<OrderStatusHistoryDTO>()
            };
        }

        

        public async Task<bool> MarkSuccessAsync(Guid orderId, Guid userId)
        {
            var orderDetail = await _shipperRepository.GetOrderDetailByIdAsync(orderId);
            if (orderDetail == null) return false;
            
            var shipper = await _shipperRepository.GetShipperByIdAsync(userId);
            if (shipper == null || shipper.Status != ShipperStatus.Approved || !shipper.IsActive || !shipper.IsAvailable) return false;
            if (orderDetail.ShipperId != shipper.Id) return false;

            // Allow from both ReadyForPickup and Shipping statuses
            if (orderDetail.Status != OrderStatus.ReadyForPickup && orderDetail.Status != OrderStatus.Shipping) 
                return false;

            // Auto confirm pickup if still in ReadyForPickup status
            if (orderDetail.Status == OrderStatus.ReadyForPickup)
            {
                orderDetail.Status = OrderStatus.Shipping;
                await _shipperRepository.AddStatusHistoryAsync(new OrderStatusHistory 
                { 
                    OrderId = orderId, 
                    Status = OrderStatus.Shipping, 
                    ChangeByUserId = userId,
                    ActionBy = OrderActionBy.Shipper,
                    ChangedAt = DateTime.UtcNow,
                    Note = "Xác nhận lấy hàng từ quán"
                });
            }

            orderDetail.Status = OrderStatus.Completed;
            orderDetail.ActualDeliveryTime = DateTime.UtcNow;

            _shipperRepository.UpdateOrderDetail(orderDetail);
            await _shipperRepository.AddStatusHistoryAsync(new OrderStatusHistory 
            { 
                OrderId = orderId, 
                Status = OrderStatus.Completed, 
                ChangeByUserId = userId,
                ActionBy = OrderActionBy.Shipper,
                ChangedAt = DateTime.UtcNow,
                Note = "Giao hàng thành công"
            });
            return await _shipperRepository.SaveChangesAsync();
        }

        public async Task<bool> MarkFailedAsync(Guid orderId, string reason, Guid? cancelledBy = null)
        {
            var orderDetail = await _shipperRepository.GetOrderDetailByIdAsync(orderId);
            if (orderDetail == null || !cancelledBy.HasValue) return false;
            
            var shipper = await _shipperRepository.GetShipperByIdAsync(cancelledBy.Value);
            if (shipper == null || shipper.Status != ShipperStatus.Approved || !shipper.IsActive || !shipper.IsAvailable) return false;
            if (orderDetail.ShipperId != shipper.Id) return false;

            // Allow from both ReadyForPickup and Shipping statuses
            if (orderDetail.Status != OrderStatus.ReadyForPickup && orderDetail.Status != OrderStatus.Shipping)
                return false;

            // Auto confirm pickup if still in ReadyForPickup status
            if (orderDetail.Status == OrderStatus.ReadyForPickup)
            {
                orderDetail.Status = OrderStatus.Shipping;
                await _shipperRepository.AddStatusHistoryAsync(new OrderStatusHistory 
                { 
                    OrderId = orderId, 
                    Status = OrderStatus.Shipping, 
                    ChangeByUserId = cancelledBy.Value,
                    ActionBy = OrderActionBy.Shipper,
                    ChangedAt = DateTime.UtcNow,
                    Note = "Xác nhận lấy hàng từ quán"
                });
            }
            
            orderDetail.Status = OrderStatus.Cancelled;
            orderDetail.CancelReason = reason ?? string.Empty;
            orderDetail.CancelledByUserId = cancelledBy.Value;
            
            _shipperRepository.UpdateOrderDetail(orderDetail);
            await _shipperRepository.AddStatusHistoryAsync(new OrderStatusHistory 
            { 
                OrderId = orderId, 
                Status = OrderStatus.Cancelled, 
                ChangeByUserId = cancelledBy.Value,
                ActionBy = OrderActionBy.Shipper,
                ChangedAt = DateTime.UtcNow,
                Note = reason ?? "Giao hàng thất bại"
            });
            return await _shipperRepository.SaveChangesAsync();
        }

        
        public async Task<Shipper?> GetShipperByIdAsync(Guid userId) => await _shipperRepository.GetShipperByIdAsync(userId);
        public async Task<List<OrderStatusHistory>> GetShipperHistoryAsync(Guid userId) => await _shipperRepository.GetShipperHistoryAsync(userId);

        public async Task<bool> ToggleShipperStatusAsync(Guid userId, bool isActive)
        {
            var shipper = await _shipperRepository.GetShipperByIdAsync(userId);
            if (shipper == null) return false;

            shipper.IsActive = isActive;
            if (!isActive)
                shipper.IsAvailable = false;
            shipper.UpdatedAt = DateTime.UtcNow;

            return await _shipperRepository.SaveChangesAsync();
        }

    

        public async Task<bool> UpdateShipperProfileAsync(Guid userId, UpdateShipperProfileDto request)
        {
            var shipper = await _shipperRepository.GetShipperByIdAsync(userId);
            if (shipper == null) return false;

            if (request.IsAvailable.HasValue)
            {
                shipper.IsAvailable = request.IsAvailable.Value;
            }
            shipper.UpdatedAt = DateTime.UtcNow;

            return await _shipperRepository.SaveChangesAsync();
        }
        
    }
}
