using FoodDelivery.Entities;
using FoodDelivery.Service.Interfaces;
using FoodDelivery.Repositories.Interfaces;
using FoodDelivery.Common;
using FoodDelivery.DTOs.Notification;
using FoodDelivery.DTOs.Shipper;
using FoodDelivery.Repositories;
using FoodDelivery.Constants;
using Microsoft.EntityFrameworkCore;
using FoodDelivery.Service.StateMachine;

namespace FoodDelivery.Service.Implements
{
    public class ShipperService : IShipperService
    {

        private readonly IShipperRepository _shipperRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly IOrderStateMachine _orderStateMachine;
        private readonly FoodContext _context;

        public ShipperService(
            IShipperRepository shipperRepository,
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            INotificationService notificationService,
            IOrderStateMachine orderStateMachine,
            FoodContext context)
        {
            _shipperRepository = shipperRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _orderStateMachine = orderStateMachine;
            _context = context;
        }

        private async Task CreateRequiredNotificationAsync(Guid userId, NotificationRequest request)
        {
            var result = await _notificationService.CreateNotificationAsync(userId, request);
            if (!result.IsSuccess)
                throw new InvalidOperationException($"Failed to create notification for user {userId}: {result.Message}");
        }

        private async Task NotifyAdminsAsync(string title, string message, string link)
        {
            try
            {
                var admins = await _userRepository.GetUsersByRoleAsync("admin");
                foreach (var admin in admins)
                {
                    await _notificationService.CreateNotificationAsync(admin.Id, new NotificationRequest
                    {
                        Title = title,
                        Message = message,
                        Type = (int)NotificationType.SYSTEM,
                        Link = link
                    });
                }
            }

            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to notify admins: {ex}");
            }
        }

        private async Task NotifyRestaurantUsersAsync(Guid restaurantId, NotificationRequest request)
        {
            var ownerIds = await _context.Restaurants
                .Where(restaurant => restaurant.Id == restaurantId)
                .Select(restaurant => restaurant.OwnerId)
                .ToListAsync();
            var staffIds = await _context.Staff
                .Where(staff => staff.IsActive && staff.RestaurantId == restaurantId)
                .Select(staff => staff.UserId)
                .ToListAsync();

            foreach (var userId in ownerIds.Concat(staffIds).Distinct())
                await CreateRequiredNotificationAsync(userId, request);
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
            var totalEarnings = orders.Where(o => o.Status == OrderStatus.Completed).Sum(o => o.Order?.ShippingFee ?? 0);
            
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
            var existing = await _shipperRepository.GetShipperByIdAsync(userId);
            if (existing != null && existing.Status != ShipperStatus.Rejected)
                return HttpErrors.BadRequest<string>("ALREADY_APPLIED", "Hồ sơ của bạn đang chờ duyệt hoặc đã được duyệt");

            var identityCard = dto.IdentityCard.Trim();
            var vehicleType = dto.VehicleType.Trim();
            var vehiclePlate = dto.VehiclePlate.Trim().ToUpperInvariant();

            if (!System.Text.RegularExpressions.Regex.IsMatch(identityCard, @"^(\d{9}|\d{12})$"))
                return HttpErrors.BadRequest<string>("INVALID_IDENTITY_CARD", "Số CCCD/CMND phải gồm 9 hoặc 12 chữ số");
            if (vehicleType.Length is < 2 or > 50)
                return HttpErrors.BadRequest<string>("INVALID_VEHICLE_TYPE", "Loại phương tiện phải từ 2 đến 50 ký tự");
            if (vehiclePlate.Length is < 5 or > 15 || !System.Text.RegularExpressions.Regex.IsMatch(vehiclePlate, @"^[0-9A-Z .-]+$"))
                return HttpErrors.BadRequest<string>("INVALID_VEHICLE_PLATE", "Biển số xe không hợp lệ");

            if (existing != null)
            {
                existing.IdentityCard = identityCard;
                existing.VehicleType = vehicleType;
                existing.VehiclePlate = vehiclePlate;
                existing.Status = ShipperStatus.Pending;
                existing.RejectReason = null;
                existing.ReviewedAt = null;
                existing.ReviewedByAdminId = null;
                existing.IsActive = false;
                existing.IsAvailable = false;
                existing.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                await NotifyAdminsAsync(
                    "Hồ sơ shipper mới",
                    "Người dùng đã gửi lại hồ sơ đăng ký shipper.",
                    "/admin/shippers");
                return Result<string>.Success("Đã gửi lại hồ sơ, vui lòng chờ admin duyệt");
            }

            var shipper = new Shipper
            {
                UserId       = userId,
                IdentityCard = identityCard,
                VehicleType  = vehicleType,
                VehiclePlate = vehiclePlate,
                Status       = ShipperStatus.Pending,
                IsActive     = false,
                IsAvailable  = false
            };

            await _shipperRepository.AddShipperAsync(shipper);
            await _context.SaveChangesAsync();

            var applicant = await _userRepository.GetUserByIdWithRoleAsync(userId);
            await NotifyAdminsAsync(
                "Hồ sơ shipper mới",
                $"{applicant?.FullName ?? "Người dùng"} đã đăng ký làm shipper.",
                "/admin/shippers");

            return Result<string>.Success("Đăng ký thành công, vui lòng chờ admin duyệt");
        }

        public async Task<Result<MyShipperApplicationDto>> GetMyApplicationAsync(Guid userId)
        {
            var shipper = await _shipperRepository.GetShipperByIdAsync(userId);
            if (shipper == null)
            {
                return Result<MyShipperApplicationDto>.Success(new MyShipperApplicationDto
                {
                    HasApplication = false
                });
            }

            return Result<MyShipperApplicationDto>.Success(new MyShipperApplicationDto
            {
                HasApplication = true,
                ShipperId = shipper.Id,
                Status = shipper.Status,
                IdentityCard = shipper.IdentityCard,
                VehicleType = shipper.VehicleType,
                VehiclePlate = shipper.VehiclePlate,
                RejectReason = shipper.RejectReason,
                AppliedAt = shipper.CreatedAt,
                ReviewedAt = shipper.ReviewedAt
            });
        }
        public async Task<Result<string>> ReviewApplicationAsync(Guid shipperId, Guid adminId, ReviewShipperDto dto)
        {
            var shipper = await _shipperRepository.GetShipperByRecordIdAsync(shipperId);
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
            await _notificationService.CreateNotificationAsync(shipper.UserId, new NotificationRequest
            {
                Title = dto.IsApproved ? "Hồ sơ shipper đã được duyệt" : "Hồ sơ shipper bị từ chối",
                Message = dto.IsApproved
                    ? "Hồ sơ đăng ký shipper của bạn đã được duyệt. Bạn có thể bắt đầu nhận đơn."
                    : $"Hồ sơ đăng ký shipper của bạn bị từ chối. Lý do: {dto.RejectReason ?? "Không có"}",
                Type = (int)NotificationType.SYSTEM,
                Link = "/shipper"
            });
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













        public async Task<Result> ConfirmPickUpAsync(Guid orderId, Guid userId)
        {
            var orderDetail = await _shipperRepository.GetOrderDetailByIdAsync(orderId);
            if (orderDetail == null)
                return HttpErrors.NotFound("ORDER_NOT_FOUND", "Không tìm thấy đơn hàng.");

            var shipper = await _shipperRepository.GetShipperByIdAsync(userId);
            if (shipper == null || shipper.Status != ShipperStatus.Approved || !shipper.IsActive)
                return HttpErrors.Forbidden("SHIPPER_NOT_ACTIVE", "Hồ sơ shipper chưa được duyệt hoặc đang tắt.");
            if (orderDetail.ShipperId != shipper.Id)
                return HttpErrors.Forbidden("SHIPPER_NOT_ASSIGNED", "Đơn hàng chưa được phân công cho bạn.");
            if (orderDetail.Status != OrderStatus.ReadyForPickup)
            {
                if (orderDetail.Status is OrderStatus.Confirmed or OrderStatus.Preparing)
                    return HttpErrors.BadRequest("FOOD_NOT_READY", "Món chưa sẵn sàng để lấy.");
                return HttpErrors.BadRequest("INVALID_STATUS", "Đơn hàng chưa ở trạng thái chờ lấy món.");
            }
            if (orderDetail.Order == null)
                return HttpErrors.InternalServerError("ORDER_DATA_INVALID", "Không thể tải thông tin đơn hàng.");

            if (!_orderStateMachine.CanTransition(
                    orderDetail.Status, OrderAction.Pickup, ActorRole.Shipper, orderDetail.Order))
                return HttpErrors.BadRequest("INVALID_TRANSITION", "Đơn hàng chưa có shipper được gán.");

            orderDetail.Status = _orderStateMachine.GetNextStatus(
                orderDetail.Status, OrderAction.Pickup, ActorRole.Shipper, orderDetail.Order);
            
            _shipperRepository.UpdateOrderDetail(orderDetail);
            await _shipperRepository.AddStatusHistoryAsync(new OrderStatusHistory  {
                OrderId = orderId,
                Status = OrderStatus.Shipping,
                ChangeByUserId = userId,
                ChangedAt = DateTime.UtcNow,
                ActionBy = OrderActionBy.Shipper,
                Note = "Shipper đã lấy món ăn."
            });
            var saved = await _shipperRepository.SaveChangesAsync();
            if (!saved)
                return HttpErrors.InternalServerError("PICKUP_SAVE_FAILED", "Không thể lưu trạng thái đã lấy hàng.");

            if (orderDetail.Order != null)
            {
                await CreateRequiredNotificationAsync(orderDetail.Order.CustomerId, new NotificationRequest
                {
                    Title = "Đơn hàng đang được giao",
                    Message = $"Shipper đã lấy đơn #{orderDetail.Order.OrderCode} và đang trên đường giao đến bạn.",
                    Type = (int)NotificationType.DELIVERY,
                    Link = "/account"
                });
            }
            return Result.Success();
        }
       public async Task<List<OrderAdminSummaryResponse>> GetAssignedOrdersAsync(Guid userId)
        {
            var shipper = await _shipperRepository.GetShipperByIdAsync(userId);
            if (shipper == null)
                return new List<OrderAdminSummaryResponse>();

            var assignedOrders = await _shipperRepository.GetOrdersByShipperAsync(shipper.Id);
            return assignedOrders
                .Where(o => o.ShipperId != null
                    && o.DispatchStatus == DispatchStatus.Assigned
                    && o.Status != OrderStatus.Completed
                    && o.Status != OrderStatus.Cancelled)
                .OrderByDescending(o => o.Order?.CreatedAt)
                .Select(o => new OrderAdminSummaryResponse
                {
                    Id = o.OrderId,
                    OrderCode = o.Order?.OrderCode ?? o.OrderId.ToString().Substring(0, 8),
                    CustomerName = o.Order?.Customer?.FullName ?? "Khách hàng",
                    TotalAmount = o.Order?.TotalAmount ?? 0,
                    ShippingFee = o.Order?.ShippingFee ?? 0,
                    ShippingAddress = o.Order?.ShippingAddress,
                    Status = o.Status.ToString(),
                    DispatchStatus = o.DispatchStatus.ToString(),
                    PrepMinutes = o.PrepMinutes,
                    CreatedAt = o.Order?.CreatedAt ?? DateTime.UtcNow
                }).ToList();
        }

        public async Task<List<OrderAdminSummaryResponse>> GetAvailableOrdersAsync(Guid userId)
        {
            var shipper = await _shipperRepository.GetShipperByIdAsync(userId);
            if (shipper == null || shipper.Status != ShipperStatus.Approved || !shipper.IsActive || !shipper.IsAvailable)
                return new List<OrderAdminSummaryResponse>();
            if (!await _context.Users.AnyAsync(user => user.Id == userId && user.IsActive))
                return new List<OrderAdminSummaryResponse>();
            if (await _context.OrderDetails.AnyAsync(detail =>
                    detail.ShipperId == shipper.Id
                    && detail.Status != OrderStatus.Completed
                    && detail.Status != OrderStatus.Cancelled))
                return new List<OrderAdminSummaryResponse>();

            var availableOrders = await _shipperRepository.GetAvailableOrdersAsync();
            return availableOrders
                .Where(o => o.Order?.CustomerId != shipper.UserId)
                .Select(o => new OrderAdminSummaryResponse
                {
                    Id = o.OrderId,
                    OrderCode = o.Order?.OrderCode ?? o.OrderId.ToString().Substring(0, 8),
                    CustomerName = o.Order?.Customer?.FullName ?? "Khách hàng",
                    TotalAmount = o.Order?.TotalAmount ?? 0,
                    ShippingFee = o.Order?.ShippingFee ?? 0,
                    ShippingAddress = o.Order?.ShippingAddress,
                    Status = o.Status.ToString(),
                    DispatchStatus = o.DispatchStatus.ToString(),
                    PrepMinutes = o.PrepMinutes,
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
                    ShippingFee = o.Order?.ShippingFee ?? 0,
                    ShippingAddress = o.Order?.ShippingAddress,
                    Status = o.Status.ToString(),
                    DispatchStatus = o.DispatchStatus.ToString(),
                    PrepMinutes = o.PrepMinutes,
                    CreatedAt = o.Order?.CreatedAt ?? DateTime.UtcNow
                }).ToList();
        }

        public async Task<Result> AcceptOrderAsync(Guid orderId, Guid shipperId)
        {
            var orderDetail = await _shipperRepository.GetOrderDetailByIdAsync(orderId);
            if (orderDetail == null)
                return HttpErrors.NotFound("ORDER_NOT_FOUND", "Không tìm thấy đơn hàng.");

            var shipper = await _shipperRepository.GetShipperByIdAsync(shipperId);
            if (shipper == null || shipper.Status != ShipperStatus.Approved || !shipper.IsActive || !shipper.IsAvailable)
                return HttpErrors.Forbidden("SHIPPER_NOT_AVAILABLE", "Hồ sơ shipper chưa được duyệt, đang tắt hoặc không nhận đơn.");
            var userIsActive = await _context.Users.AnyAsync(user => user.Id == shipperId && user.IsActive);
            if (!userIsActive)
                return HttpErrors.Forbidden("SHIPPER_NOT_AVAILABLE", "Tài khoản shipper không hoạt động.");
            if (orderDetail.Order?.CustomerId == shipper.UserId)
                return HttpErrors.Forbidden(
                    "SHIPPER_CANNOT_ACCEPT_OWN_ORDER",
                    "Bạn không thể nhận giao đơn hàng do chính mình đặt.");

            await using var transaction = await _context.Database.BeginTransactionAsync();
            var assignedAt = DateTime.UtcNow;
            var claimed = await _shipperRepository.TryAssignAvailableOrderAsync(orderId, shipper.Id, assignedAt);
            if (claimed == 0)
                return HttpErrors.BadRequest(
                    "SHIPPER_BUSY_OR_ORDER_UNAVAILABLE",
                    "Bạn đang có đơn cần xử lý hoặc đơn này không còn khả dụng.");

            orderDetail.ShipperId = shipper.Id;
            orderDetail.DispatchStatus = DispatchStatus.Assigned;
            orderDetail.ShipperAssignedAt = assignedAt;
            await _shipperRepository.AddStatusHistoryAsync(new OrderStatusHistory
            {
                OrderId = orderId,
                Status = orderDetail.Status,
                ChangeByUserId = shipperId,
                ChangedAt = assignedAt,
                ActionBy = OrderActionBy.Shipper,
                Note = OrderStatusHistory.ShipperAcceptedNote
            });

            await _context.SaveChangesAsync();

            if (orderDetail.Order != null)
            {
                await CreateRequiredNotificationAsync(orderDetail.Order.CustomerId, new NotificationRequest
                {
                    Title = "Shipper đã nhận đơn",
                    Message = $"Đơn #{orderDetail.Order.OrderCode} đã có shipper nhận giao.",
                    Type = (int)NotificationType.DELIVERY,
                    Link = "/account"
                });
                await NotifyRestaurantUsersAsync(orderDetail.Order.RestaurantId, new NotificationRequest
                {
                    Title = "Shipper đã nhận đơn",
                    Message = $"Shipper đã nhận giao đơn #{orderDetail.Order.OrderCode}.",
                    Type = (int)NotificationType.DELIVERY,
                    Link = "/restaurant/orders"
                });
            }

            await transaction.CommitAsync();

            return Result.Success();
        }

        public async Task<Result> ReleaseOrderAsync(Guid orderId, Guid shipperId, string reason)
        {
            var orderDetail = await _shipperRepository.GetOrderDetailByIdAsync(orderId);
            if (orderDetail == null)
                return HttpErrors.NotFound("ORDER_NOT_FOUND", "Không tìm thấy đơn hàng.");

            var shipper = await _shipperRepository.GetShipperByIdAsync(shipperId);
            if (shipper == null || shipper.Status != ShipperStatus.Approved || !shipper.IsActive)
                return HttpErrors.Forbidden("FORBIDDEN", "Bạn không phải shipper đang hoạt động.");

            var cleanReason = string.IsNullOrWhiteSpace(reason)
                ? "Shipper không thể tiếp tục nhận đơn trước khi lấy hàng."
                : reason.Trim();
            var releasedAt = DateTime.UtcNow;

            await using var transaction = await _context.Database.BeginTransactionAsync();
            var released = await _context.OrderDetails
                .Where(detail => detail.OrderId == orderId
                    && detail.ShipperId == shipper.Id
                    && (detail.Status == OrderStatus.Confirmed
                        || detail.Status == OrderStatus.Preparing
                        || detail.Status == OrderStatus.ReadyForPickup)
                    && detail.DispatchStatus == DispatchStatus.Assigned)
                .ExecuteUpdateAsync(updates => updates
                    .SetProperty(detail => detail.ShipperId, (Guid?)null)
                    .SetProperty(detail => detail.ShipperAssignedAt, (DateTime?)null)
                    .SetProperty(detail => detail.DispatchStatus, DispatchStatus.Searching)
                    .SetProperty(detail => detail.DispatchStartedAt, releasedAt)
                    .SetProperty(detail => detail.ShipperReleaseCount, detail => detail.ShipperReleaseCount + 1)
                    .SetProperty(detail => detail.LastShipperReleaseReason, cleanReason));
            if (released == 0)
                return HttpErrors.BadRequest(
                    "ORDER_CANNOT_BE_RELEASED",
                    "Chỉ shipper được gán mới có thể trả đơn trước khi lấy món.");

            orderDetail.ShipperId = null;
            orderDetail.Shipper = null;
            orderDetail.ShipperAssignedAt = null;
            orderDetail.DispatchStatus = DispatchStatus.Searching;
            orderDetail.DispatchStartedAt = releasedAt;
            orderDetail.ShipperReleaseCount++;
            orderDetail.LastShipperReleaseReason = cleanReason;
            await _shipperRepository.AddStatusHistoryAsync(new OrderStatusHistory
            {
                OrderId = orderId,
                Status = orderDetail.Status,
                ChangeByUserId = shipperId,
                ChangedAt = releasedAt,
                ActionBy = OrderActionBy.Shipper,
                Note = $"Shipper trả đơn để tìm người giao khác. Lý do: {cleanReason}"
            });
            await _context.SaveChangesAsync();

            if (orderDetail.Order != null)
            {
                await NotifyRestaurantUsersAsync(orderDetail.Order.RestaurantId, new NotificationRequest
                {
                    Title = "Đơn hàng cần tìm shipper khác",
                    Message = $"Shipper đã trả đơn #{orderDetail.Order.OrderCode} trước khi lấy món. Hệ thống đang tìm shipper khác. Lý do: {cleanReason}",
                    Type = (int)NotificationType.DELIVERY,
                    Link = "/restaurant/orders"
                });
                await NotifyEligibleShippersAsync(orderDetail.Order);
            }

            await transaction.CommitAsync();
            return Result.Success();
        }

        private async Task NotifyEligibleShippersAsync(Order order)
        {
            var userIds = await _context.Shippers
                .Where(shipper => shipper.Status == ShipperStatus.Approved
                    && shipper.IsActive
                    && shipper.IsAvailable
                    && shipper.User.IsActive
                    && !_context.OrderDetails.Any(activeOrder =>
                        activeOrder.ShipperId == shipper.Id
                        && activeOrder.Status != OrderStatus.Completed
                        && activeOrder.Status != OrderStatus.Cancelled))
                .Select(shipper => shipper.UserId)
                .ToListAsync();

            foreach (var userId in userIds)
            {
                await CreateRequiredNotificationAsync(userId, new NotificationRequest
                {
                    Title = "Có đơn hàng mới cần giao",
                    Message = $"Đơn #{order.OrderCode} đang cần shipper nhận giao.",
                    Type = (int)NotificationType.DELIVERY,
                    Link = "/shipper/orders"
                });
            }
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
            if (shipper == null || shipper.Status != ShipperStatus.Approved || !shipper.IsActive) return false;
            if (orderDetail.ShipperId != shipper.Id) return false;
            if (orderDetail.Status != OrderStatus.Shipping || orderDetail.Order == null)
                return false;
            if (!_orderStateMachine.CanTransition(
                    orderDetail.Status, OrderAction.Deliver, ActorRole.Shipper, orderDetail.Order))
                return false;

            orderDetail.Status = _orderStateMachine.GetNextStatus(
                orderDetail.Status, OrderAction.Deliver, ActorRole.Shipper, orderDetail.Order);
            orderDetail.ActualDeliveryTime = DateTime.UtcNow;
            shipper.TotalDeliveredOrders++;

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
            var saved = await _shipperRepository.SaveChangesAsync();
            if (saved && orderDetail.Order != null)
            {
                await CreateRequiredNotificationAsync(orderDetail.Order.CustomerId, new NotificationRequest
                {
                    Title = "Đơn hàng đã được giao",
                    Message = $"Đơn hàng #{orderDetail.Order.OrderCode} của bạn đã được giao thành công!",
                    Type = (int)NotificationType.DELIVERY,
                    Link = "/account"
                });
            }
            return saved;
        }

        public async Task<bool> MarkFailedAsync(Guid orderId, string reason, Guid? cancelledBy = null)
        {
            var orderDetail = await _shipperRepository.GetOrderDetailByIdAsync(orderId);
            if (orderDetail == null || !cancelledBy.HasValue) return false;
            
            var shipper = await _shipperRepository.GetShipperByIdAsync(cancelledBy.Value);
            if (shipper == null || shipper.Status != ShipperStatus.Approved || !shipper.IsActive) return false;
            if (orderDetail.ShipperId != shipper.Id) return false;
            if (orderDetail.Status != OrderStatus.Shipping || orderDetail.Order == null)
                return false;

            if (!_orderStateMachine.CanTransition(
                    orderDetail.Status, OrderAction.DeliveryFailed, ActorRole.Shipper, orderDetail.Order))
                return false;

            orderDetail.Status = _orderStateMachine.GetNextStatus(
                orderDetail.Status, OrderAction.DeliveryFailed, ActorRole.Shipper, orderDetail.Order);
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
            var saved = await _shipperRepository.SaveChangesAsync();
            if (saved && orderDetail.Order != null)
            {
                await CreateRequiredNotificationAsync(orderDetail.Order.CustomerId, new NotificationRequest
                {
                    Title = "Giao hàng không thành công",
                    Message = $"Đơn #{orderDetail.Order.OrderCode} chưa giao được. Lý do: {reason ?? "Không có"}",
                    Type = (int)NotificationType.DELIVERY,
                    Link = "/account"
                });
                var admins = await _userRepository.GetUsersByRoleAsync("admin");
                foreach (var admin in admins)
                {
                    await CreateRequiredNotificationAsync(admin.Id, new NotificationRequest
                    {
                        Title = "Đơn hàng giao thất bại",
                        Message = $"Shipper báo giao thất bại đơn #{orderDetail.Order.OrderCode}. Lý do: {reason ?? "Không có"}",
                        Type = (int)NotificationType.SYSTEM,
                        Link = "/admin/orders"
                    });
                }
            }
            return saved;
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
