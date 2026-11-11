using FoodDelivery.Common;
using FoodDelivery.DTOs.Order;
using FoodDelivery.DTOs.Notification;
using FoodDelivery.Entities;
using FoodDelivery.Repositories.Interfaces;
using FoodDelivery.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using FoodDelivery.Service.Implements.StateMachine;
using FoodDelivery.Service.StateMachine;
using FoodDelivery.Repositories;
using FoodDelivery.Constants;
using FoodDelivery.Service.Interfaces.IShipping;
using System.Data;

namespace FoodDelivery.Service.Implementations;
public class OrderService :IOrderService
{
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ICartItemRepository _cartItemRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly INotificationService _notificationService;
    private readonly IUserRepository _userRepository;
    private readonly IShipperRepository _shipperRepository;
    private readonly IOrderStateMachine _orderStateMachine;
    private readonly IRoleRepository _roleRepository;
    private readonly IStaffRepository _staffRepository;
    private readonly IShippingEstimationService _shippingEstimation;
    private readonly IVnPayService _vnPayService;
    private readonly IWalletService _walletService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly FoodContext _context;
    public OrderService (
        IProductRepository productRepo,
        IOrderRepository orderRepo,  
        ICartItemRepository cartItemRepo,
        IAddressRepository addressRepo,
        IRestaurantRepository restaurantRepository,
        INotificationService notificationService,
        IUserRepository userRepository,
        IShipperRepository shipperRepository,
        IOrderStateMachine orderStateMachine,
        IRoleRepository roleRepository,
        IStaffRepository staffRepository,
        IShippingEstimationService shippingEstimation,
        IVnPayService vnPayService,
        IWalletService walletService,
        IHttpContextAccessor httpContextAccessor,
        FoodContext context)
    {
        _productRepository = productRepo;
        _orderRepository = orderRepo;
        _orderStateMachine = orderStateMachine;
        _context = context;
        _cartItemRepository = cartItemRepo;
        _addressRepository = addressRepo;
        _restaurantRepository = restaurantRepository;
        _notificationService = notificationService;
        _userRepository = userRepository;
        _shipperRepository = shipperRepository;
        _roleRepository = roleRepository;
        _staffRepository = staffRepository;
        _shippingEstimation = shippingEstimation;
        _vnPayService = vnPayService;
        _walletService = walletService;
        _httpContextAccessor = httpContextAccessor;
    }

    private async Task NotifyRestaurantUsersAsync(Guid restaurantId, NotificationRequest request)
    {
        var recipientIds = await _context.Restaurants
            .Where(restaurant => restaurant.Id == restaurantId)
            .Select(restaurant => restaurant.OwnerId)
            .ToListAsync();
        var staffIds = await _context.Staff
            .Where(staff => staff.IsActive && staff.RestaurantId == restaurantId)
            .Select(staff => staff.UserId)
            .ToListAsync();

        foreach (var recipientId in recipientIds.Concat(staffIds).Distinct())
            await CreateRequiredNotificationAsync(recipientId, request);
    }

    private async Task CreateRequiredNotificationAsync(Guid userId, NotificationRequest request)
    {
        var result = await _notificationService.CreateNotificationAsync(userId, request);
        if (!result.IsSuccess)
            throw new InvalidOperationException($"Failed to create notification for user {userId}: {result.Message}");
    }

    private async Task NotifyDispatchShippersAsync(Order order)
    {
        var shipperUserIds = await _context.Shippers
            .Where(shipper => shipper.Status == ShipperStatus.Approved
                && shipper.IsActive
                && shipper.IsAvailable
                && shipper.User.IsActive
                && shipper.UserId != order.CustomerId
                && !_context.OrderDetails.Any(activeOrder =>
                    activeOrder.ShipperId == shipper.Id
                    && activeOrder.Status != OrderStatus.Completed
                    && activeOrder.Status != OrderStatus.Cancelled))
            .Select(shipper => shipper.UserId)
            .ToListAsync();

        foreach (var shipperUserId in shipperUserIds)
        {
            await CreateRequiredNotificationAsync(shipperUserId, new NotificationRequest
            {
                Title = "Có đơn hàng mới cần giao",
                Message = $"Đơn #{order.OrderCode} tại nhà hàng đang chờ shipper nhận giao.",
                Type = (int)NotificationType.DELIVERY,
                Link = "/shipper/orders"
            });
        }
    }

    private async Task NotifyAssignedShipperAsync(Order order, string title, string message)
    {
        if (order.OrderDetail.ShipperId is not Guid shipperId)
            return;

        var shipperUserId = await _context.Shippers
            .Where(shipper => shipper.Id == shipperId)
            .Select(shipper => (Guid?)shipper.UserId)
            .FirstOrDefaultAsync();
        if (shipperUserId is not Guid userId)
            throw new InvalidOperationException($"Assigned shipper {shipperId} has no user account.");

        await CreateRequiredNotificationAsync(userId, new NotificationRequest
        {
            Title = title,
            Message = message,
            Type = (int)NotificationType.DELIVERY,
            Link = "/shipper/orders"
        });
    }
    
    /// Resolve ActorRole từ userId bằng cách query UserRoles.
    private async Task<ActorRole?> ResolveActorRoleAsync(Guid userId)
    {
        var roles = await _roleRepository.GetRolesByUserIdAsync(userId);
        var roleIds = roles.Select(r => r.Id).ToList();
        if (roleIds.Contains(RoleIds.Admin))           return ActorRole.Admin;
        if (roleIds.Contains(RoleIds.RestaurantOwner)) return ActorRole.RestaurantOwner;
        if (roleIds.Contains(RoleIds.RestaurantStaff)) return ActorRole.RestaurantStaff;
        if (roleIds.Contains(RoleIds.Shipper))         return ActorRole.Shipper;
        if (roleIds.Contains(RoleIds.Customer))        return ActorRole.Customer;
        return null;
    }

    /// Kiểm tra user có thuộc nhà hàng này không.
    /// - Owner: check Restaurant.OwnerId
    /// - Staff: check Staff table
    private async Task<bool> IsUserInRestaurantAsync(Guid userId, Guid restaurantId, ActorRole actorRole)
    {
        if (actorRole == ActorRole.RestaurantOwner)
        {
            var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
            return restaurant != null && restaurant.OwnerId == userId;
        }
        else if (actorRole == ActorRole.RestaurantStaff)
        {
            var isStaff = await _staffRepository.IsStaffOfRestaurantAsync(userId, restaurantId);
            return isStaff;
        }
        return false;
    }
    /// Áp dụng transition — thay thế mọi if (status != X) rải rác.
    /// Tự động: check state machine → cập nhật status → ghi history → chạy side effect.
    private async Task<Result> ApplyTransitionAsync(
        Order order,
        OrderAction action,
        ActorRole actor,
        Guid actorUserId,
        string historyNote,
        Func<Order, Task>? onSuccess = null,
        Action<Order>? beforeSave = null,
        bool transactionalSideEffects = false)
    {
        var current = order.OrderDetail.Status;

        if (!_orderStateMachine.CanTransition(current, action, actor, order))
        {
            return HttpErrors.BadRequest(
                "INVALID_STATUS",
                $"Không thể thực hiện '{action}' ở trạng thái '{current}'.");
        }

        var next = _orderStateMachine.GetNextStatus(current, action, actor, order);
        order.OrderDetail.Status = next;
        if (next == OrderStatus.Confirmed
            && action is OrderAction.Confirm or OrderAction.AcceptProposal)
        {
            order.OrderDetail.DispatchStatus = DispatchStatus.Searching;
            order.OrderDetail.DispatchStartedAt = DateTime.UtcNow;
        }
        if (next == OrderStatus.Cancelled
            && action is OrderAction.CancelByCustomer or OrderAction.CancelByAdmin or OrderAction.CancelByRestaurant
            && order.OrderDetail.ShipperId is not null)
        {
            order.OrderDetail.ShipperCancellationCompensationPending = true;
        }
        beforeSave?.Invoke(order);

        await _orderRepository.AddOrderHistoryAsync(new OrderStatusHistory
        {
            OrderId = order.Id,
            Status = next,
            ChangeByUserId = actorUserId,
            ChangedAt = DateTime.UtcNow,
            Note = historyNote,
            ActionBy = MapActor(actor)
        });

        await using var transaction = transactionalSideEffects || next == OrderStatus.Cancelled
            ? await _context.Database.BeginTransactionAsync()
            : null;

        try
        {
            if (next == OrderStatus.Cancelled && order.OrderDetail.PaymentStatus == PaymentStatus.Paid)
            {
                order.OrderDetail.PaymentStatus = PaymentStatus.Refunded;
                await _walletService.CreditAsync(
                    order.CustomerId,
                    order.TotalAmount,
                    WalletTransactionType.OrderRefund,
                    order.Id,
                    $"Hoàn tiền đơn hàng {order.OrderCode} đã bị hủy.");
            }

            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (transaction is not null)
                await transaction.RollbackAsync();
            return HttpErrors.Conflict("ORDER_CHANGED", "Đơn hàng vừa được cập nhật. Vui lòng tải lại và thử lại.");
        }

        if (onSuccess != null)
            await onSuccess(order);

        if (transaction is not null)
            await transaction.CommitAsync();

        return Result.Success();
    }
    /// Shortcut: tìm order theo id rồi apply transition.
    private async Task<Result> TransitionByIdAsync(
        Guid orderId,
        OrderAction action,
        ActorRole actor,
        Guid actorUserId,
        string historyNote,
        Func<Order, Task>? onSuccess = null)
    {
        var order = await _orderRepository.GetOrderById(orderId);
        if (order == null)
            return HttpErrors.NotFound("ORDER_NOT_FOUND", "Không tìm thấy đơn hàng.");

        return await ApplyTransitionAsync(
            order, action, actor, actorUserId, historyNote, onSuccess);
    }
    private static OrderActionBy MapActor(ActorRole actor) => actor switch
    {
        ActorRole.Customer        => OrderActionBy.Customer,
        ActorRole.Admin           => OrderActionBy.Admin,
        ActorRole.Shipper         => OrderActionBy.Shipper,
        ActorRole.RestaurantOwner => OrderActionBy.RestaurantOwner,
        ActorRole.RestaurantStaff => OrderActionBy.RestaurantStaff,
        _                         => OrderActionBy.System
    };
    public async Task<Result<CreateOrderResponseDto>> BuyNowAsync(Guid customerId,BuyNowRequestDto request)
    {
        if(request.Quantity <= 0)
        {
            return HttpErrors.BadRequest<CreateOrderResponseDto>("INVALID_QUANTITY", "Số lượng phải lớn hơn 0");
        }
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        if(product == null)
        {
            return HttpErrors.NotFound<CreateOrderResponseDto>("PRODUCT_NOT_FOUND", "Không tìm thấy sản phẩm.");
        }
        if (!product.IsAvailable)
        {
            return HttpErrors.BadRequest<CreateOrderResponseDto>("PRODUCT_UNAVAILABLE", "Đơn hang đang có sản phẩm ngừng bán");
        }
        var statusRestaurant = await _restaurantRepository.GetByIdAsync(product.RestaurantId);
        if(statusRestaurant != null &&statusRestaurant.IsOpen == false)
        {
            return HttpErrors.BadRequest<CreateOrderResponseDto>("RESTAURANT_CLOSE", statusRestaurant.ClosingMessage ?? "Quán tạm nghỉ. Bạn có thể quay lại vào hôm sau.");
        }
        var item = new List<CreateOrderItemDto>
        {
            new CreateOrderItemDto()
            {
                ProductId = product.Id,
                // ProductImage = product.ImageUrl,
                ProductName = product.Name,
                Quantity  = request.Quantity,
                UnitPrice = product.Price,
                UnitCostSnapshot = product.CostPrice,

            }
        };
        return await CreateOrderInternalAsync(
            customerId,
            request.AddressId,
            product.RestaurantId,
            item,
            request.Note,
            request.ShippingService,
            request.PaymentMethod);

    }
    public async Task<Result<CreateOrderResponseDto>> CheckoutAsync(Guid customerId,CheckoutRequestDto request)
    {
        var cartItems = await _cartItemRepository.GetByIdsAsync(customerId,request.CartItemIds);
        if (cartItems == null || !cartItems.Any())
        {
            return HttpErrors.BadRequest<CreateOrderResponseDto>("CART_EMPTY", "Đơn hàng phải có ít nhất một sản phẩm");
        }
        if (cartItems.Any(ci => ci.Quantity <= 0))
        {
            return HttpErrors.BadRequest<CreateOrderResponseDto>("INVALID_QUANTITY", "Số lượng sản phẩm không hợp lệ");
        }
        if (cartItems.Any(ci=>ci.Cart.CustomerId != customerId))
        {
            return HttpErrors.Forbidden<CreateOrderResponseDto>("FORBIDDEN", "Không có quyền thanh toán giỏ hàng này");
        }
        if(cartItems.Any(ci=>ci.Product.IsAvailable == false))
        {
            return HttpErrors.BadRequest<CreateOrderResponseDto>("PRODUCT_UNAVAILABLE", "Đơn hang đang có sản phẩm ngừng bán");
        }

        var restaurantIds = cartItems.Select(ci => ci.Product.RestaurantId).Distinct().ToList();
        if (restaurantIds.Count > 1)
            return HttpErrors.BadRequest<CreateOrderResponseDto>("MIXED_RESTAURANTS", "Không thể đặt hàng từ nhiều quán cùng lúc");

        var restaurantId = restaurantIds.First();

         var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
        if (restaurant == null)
            return HttpErrors.BadRequest<CreateOrderResponseDto>("RESTAURANT_NOT_CONFIGURED", "Cửa hàng chưa được cấu hình.");
        if (!restaurant.IsOpen)
            return HttpErrors.BadRequest<CreateOrderResponseDto>("RESTAURANT_CLOSE", restaurant.ClosingMessage ?? "Quán tạm nghỉ.");
        var items = cartItems.Select(ci=> new CreateOrderItemDto
        {
                ProductId = ci.Product.Id,
                ProductName = ci.Product.Name,
                Quantity  = ci.Quantity,
                UnitPrice = ci.Product.Price,
                UnitCostSnapshot = ci.Product.CostPrice,
        }).ToList();
        var result = await CreateOrderInternalAsync(
            customerId,
            request.AddressId,
            restaurantId,
            items,
            request.Note,
            request.ShippingService,
            request.PaymentMethod);
        if (result.IsSuccess)
        {
            await _cartItemRepository.DeleteRangeAsync(cartItems);
            await _context.SaveChangesAsync();
        }
        return result;
    }

    public async Task<Result<ShippingFeeResponseDto>> CalculateShippingFeeAsync(
        Guid addressId,
        Guid restaurantId,
        decimal subtotal,
        string serviceType = "Standard")
    {
        if (!IsSupportedShippingService(serviceType))
            return HttpErrors.BadRequest<ShippingFeeResponseDto>("UNKNOWN_SERVICE", "Kiểu giao hàng không được hỗ trợ.");

        var est = await _shippingEstimation.EstimateAsync(
        addressId, restaurantId, serviceType, subtotal, itemCount: 1);

        if (!est.IsSuccess)
            return HttpErrors.BadRequest<ShippingFeeResponseDto>(est.ErrorCode ?? "SHIPPING_FEE_ERROR", est.Message ?? "Lỗi khi tính phí vận chuyển");
        if (est.Data == null)
            return HttpErrors.InternalServerError<ShippingFeeResponseDto>("SHIPPING_FEE_ERROR", "Không nhận được dữ liệu phí vận chuyển");
        return Result<ShippingFeeResponseDto>.Success(new ShippingFeeResponseDto
        {
            ShippingFee = est.Data.Fee,
            Distance = est.Data.Route.DistanceKm,
            EstimatedMinutes = est.Data.Route.DurationMinutes
        });
    }
    private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        var R = 6371; //Bán kính trái đất;
        var dLat = (lat1 - lat2) * (Math.PI/180);
        var dLon = (lon1 - lon2) * (Math.PI/180);
        var a = Math.Sin(dLat/2) * Math.Sin(dLat/2)
            + Math.Cos(lat1* Math.PI/180)*Math.Cos(lat2* Math.PI/180) 
            * Math.Sin(dLon/2)*Math.Sin(dLon/2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a));
        var d = R * c;
        return d *1.2; //Giả sử hệ số đường bộ là 1.2
    }
    private string GenerateOrderCode() => $"FD{DateTime.UtcNow.Ticks}";
    private static bool IsSupportedShippingService(string? serviceType) =>
        string.Equals(serviceType, "Standard", StringComparison.OrdinalIgnoreCase)
        || string.Equals(serviceType, "Express", StringComparison.OrdinalIgnoreCase);

    private async Task<Result<CreateOrderResponseDto>> CreateOrderInternalAsync(
        Guid customerId, Guid addressId,
        Guid restaurantId,
        List<CreateOrderItemDto> items,
        string? note,
        string shippingService,
        PaymentMethod paymentMethod)
    {
        // Cho phép cả Cash và VnPay
        if (paymentMethod != PaymentMethod.Cash && paymentMethod != PaymentMethod.VnPay)
            return HttpErrors.BadRequest<CreateOrderResponseDto>("PAYMENT_METHOD_UNAVAILABLE", "Phương thức thanh toán chưa được hỗ trợ.");
        if (!IsSupportedShippingService(shippingService))
            return HttpErrors.BadRequest<CreateOrderResponseDto>("UNKNOWN_SERVICE", "Kiểu giao hàng không được hỗ trợ.");

        var address = await _addressRepository.GetByIdAsync(addressId);
        if(address == null)
        {
            return HttpErrors.NotFound<CreateOrderResponseDto>("ADDRESS_NOT_FOUND", "Không tìm thấy địa chỉ");
        }
        var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
        if(restaurant== null)
        {
            return HttpErrors.BadRequest<CreateOrderResponseDto>("RESTAURANT_NOT_CONFIGURED", "Cửa hàng chưa được cấu hình.");
        }
        var subtotal = items.Sum(i => i.UnitPrice * i.Quantity);
        var est = await _shippingEstimation.EstimateAsync(addressId, restaurantId, shippingService, subtotal, items.Count);
        if (!est.IsSuccess)
            return HttpErrors.BadRequest<CreateOrderResponseDto>(est.ErrorCode ?? "SHIPPING_FEE_ERROR", est.Message ?? "Lỗi khi tính phí vận chuyển");
        if (est.Data == null)
            return HttpErrors.InternalServerError<CreateOrderResponseDto>("SHIPPING_FEE_ERROR", "Không nhận được dữ liệu phí vận chuyển");
        var estimatedDeliveryTime = DateTime.UtcNow.AddMinutes(
            est.Data.Route.DurationMinutes + 10 + Math.Max(0, items.Count - 1) * 2);
        var order = new Order()
        {
            CustomerId = customerId,
            Note = note,
            RestaurantId = restaurantId,
            Subtotal = subtotal,
            TotalAmount = subtotal + est.Data.Fee,
            ShippingFee = est.Data.Fee,
            CreatedAt = DateTime.UtcNow,
            OrderCode = GenerateOrderCode(),
            ReceiverName = address.ReceiverName,
            ShippingAddress = address.FullAddress,
            ReceiverPhone = address.PhoneNumber,
        };
        foreach(var item in items)
        {
            order.OrderItems.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                UnitCostSnapshot = item.UnitCostSnapshot,
                ProductImage = item.ProductImage,
                ProductName = item.ProductName,
            });
        }
        order.OrderDetail = new OrderDetail
        {
            OrderId = order.Id,
            Status = OrderStatus.Pending,
            PaymentMethod = paymentMethod,
            PaymentStatus = PaymentStatus.Unpaid,
            EstimatedDeliveryTime = estimatedDeliveryTime
        };
        order.OrderStatusHistories.Add(new OrderStatusHistory()
        {
          OrderId = order.Id,
          ChangeByUserId = customerId,
          Status = OrderStatus.Pending,
          ChangedAt = DateTime.UtcNow,
          ActionBy = OrderActionBy.Customer,
          Note = "Bạn đã tạo đơn hàng."

        });
        string? paymentUrl = null;
        if (paymentMethod == PaymentMethod.VnPay)
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("VNPay payment URLs can only be created for an HTTP request.");
            paymentUrl = _vnPayService.CreatePaymentUrl(order, httpContext);
        }

        await _orderRepository.AddAsync(order);
        await _context.SaveChangesAsync();

        try
    {
        var admins = await _userRepository.GetUsersByRoleAsync("admin");
        foreach (var admin in admins)
        {
            await _notificationService.CreateNotificationAsync(admin.Id, new NotificationRequest
            {
                Title = "Đơn hàng mới",
                Message = $"Khách hàng vừa đặt đơn #{order.OrderCode}. Tổng tiền: {order.TotalAmount:N0} VND",
                Type = (int)NotificationType.ORDER,
                Link = "/admin/orders"
            });
        }

        //  Chỉ notify staff của quán đó
        var staffMembers = await _context.Staff
            .Include(s => s.User)
            .Where(s => s.IsActive && s.RestaurantId == restaurantId)
            .ToListAsync();

        foreach (var staff in staffMembers)
        {
            await _notificationService.CreateNotificationAsync(staff.UserId, new NotificationRequest
            {
                Title = "Đơn hàng mới cần xử lý",
                Message = $"Khách hàng vừa đặt đơn #{order.OrderCode}. Tổng tiền: {order.TotalAmount:N0} VND",
                Type = (int)NotificationType.ORDER,
                Link = "/restaurant/orders"
            });
        }

        var restaurantOwnerId = await _context.Restaurants
            .Where(restaurant => restaurant.Id == restaurantId)
            .Select(restaurant => restaurant.OwnerId)
            .FirstOrDefaultAsync();
        if (restaurantOwnerId != Guid.Empty && !staffMembers.Any(staff => staff.UserId == restaurantOwnerId))
        {
            await _notificationService.CreateNotificationAsync(restaurantOwnerId, new NotificationRequest
            {
                Title = "Đơn hàng mới cần xử lý",
                Message = $"Khách hàng vừa đặt đơn #{order.OrderCode}. Tổng tiền: {order.TotalAmount:N0} VND",
                Type = (int)NotificationType.ORDER,
                Link = "/restaurant/orders"
            });
        }
    }
        catch (Exception ex)
        {
            Console.WriteLine($"[Notification] Error sending notification: {ex.Message}");
            Console.WriteLine($"[Notification] Stack trace: {ex.StackTrace}");
        }

        return Result<CreateOrderResponseDto>.Success(new CreateOrderResponseDto
        {
            OrderCode = order.OrderCode,
            OrderId = order.Id,
            Status = order.OrderDetail.Status,
            Subtotal = order.Subtotal,
            TotalAmount = order.TotalAmount,
            ShippingFee = order.ShippingFee,
            EstimatedDeliveryTime = estimatedDeliveryTime,
            PaymentUrl = paymentUrl
        });
    }
    public async Task<Result<PagedResponse<OrderHistoryItemResponse>>> GetMyOrderAsync(Guid customerId, int page, int pageSize)
    {
        pageSize = pageSize > 100 ? 100 : pageSize < 1 ? 10 : pageSize;
        page = page < 1 ? 1 : page;
        var order = await _orderRepository.GetMyOrderAsync(customerId,page,pageSize);
        var totalCount = await _orderRepository.GetTotalCountAsync(customerId);
        var responseMeta = new PaginationMeta(
            Page: page,
            PageSize: pageSize,
            TotalCount: totalCount,
            TotalPages: (int)Math.Ceiling((double)totalCount / pageSize)
        );
        var pageResponse = new PagedResponse<OrderHistoryItemResponse>(
            Data: order,
            Meta: responseMeta
        );
        return Result<PagedResponse<OrderHistoryItemResponse>>.Success(pageResponse);
    }
    public async Task<Result<OrderDetailResponse>> GetOrderDetailAsync(Guid userId, List<string> roles, Guid orderId)
    {   
        try
        {
            var order = await _orderRepository.GetOrderDetailAsync(orderId);
            if(order == null)
            {
                return HttpErrors.NotFound<OrderDetailResponse>("ORDER__NOT_FOUND", "Không tìm thấy đơn hàng này");
            }
            bool isAdmin = roles.Contains("Admin");
            bool isOwner = order.CustomerId == userId;
            if (!isAdmin && !isOwner)
            {
                return HttpErrors.Forbidden<OrderDetailResponse>("ORDER__FORBIDDEN", "Bạn không có quyền xem đơn hàng này");
            }
            
            // Check if OrderDetail exists
            if (order.OrderDetail == null)
            {
                return HttpErrors.NotFound<OrderDetailResponse>("ORDER_DETAIL_NOT_FOUND", "Thông tin chi tiết đơn hàng không tồn tại");
            }
            
            var response = new OrderDetailResponse
            {
                CustomerId = order.CustomerId,
                CustomerEmail = order.Customer?.Email ?? "",
                CustomerName = order.Customer?.FullName ?? "",
                OrderId = order.Id,
                OrderCode = order.OrderCode,
                CreatedAt = order.CreatedAt,
                ShippingAddress  = order.ShippingAddress,
                ReceiverName  = order.ReceiverName,
                ReceiverPhone  = order.ReceiverPhone,
                ShippingFee = order.ShippingFee, 
                EstimatedDeliveryTime  = order.OrderDetail.EstimatedDeliveryTime,
                ActualDeliveryTime = order.OrderDetail.ActualDeliveryTime,
                PaymentMethod = order.OrderDetail.PaymentMethod,
                PaymentStatus = order.OrderDetail.PaymentStatus,
                CurrentStatus =order.OrderDetail.Status,
                CancelReason = order.OrderDetail.CancelReason,
                Subtotal = order.Subtotal,
                TotalAmount = order.TotalAmount,
            };
        foreach(var item in order.OrderItems)
        {
            response.Items.Add(new OrderItemResponse
            {
                OrderItemId = item.Id,
                ProductId = item.ProductId,
                ProductName = item.ProductName ?? string.Empty,
                ProductImage  = item.ProductImage ?? string.Empty,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.UnitPrice * item.Quantity,
                IsRemoved = item.IsRemoved,
                RemoveReason = item.RemoveReason ?? string.Empty,
            });
        }
        foreach(var history in order.OrderStatusHistories.OrderBy(history => history.ChangedAt))
        {
            response.StatusHistories.Add(new OrderStatusHistoryResponse
            {
                Status = history.Status,
                ChangedAt = history.ChangedAt,
                ChangedBy  = history.ChangeByUser != null
                    ? history.ChangeByUser.FullName
                    : "System",
                Note  = history.Note ?? string.Empty,
            });
        }
        return Result<OrderDetailResponse>.Success(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] GetOrderDetailAsync failed: {ex.Message}");
            Console.WriteLine($"[ERROR] StackTrace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"[ERROR] InnerException: {ex.InnerException.Message}");
            }
            return HttpErrors.InternalServerError<OrderDetailResponse>("ORDER_ERROR", $"Lỗi: {ex.Message}");
        }
    }   
    public async Task<Result> CancelOrderByCustomerAsync(Guid userId,Guid orderId, CancelOrderRequestDto request)
    {
        var reason = request.Reason?.Trim();
        if (string.IsNullOrWhiteSpace(reason) || reason.Length > 300)
            return HttpErrors.BadRequest("INVALID_CANCEL_REASON", "Vui lòng nhập lý do hủy từ 1 đến 300 ký tự.");

        var order = await _orderRepository.GetOrderById(orderId);
        if(order == null)
        {
            return HttpErrors.NotFound("ORDER_NOT_FOUND", "Không tìm thấy đơn hàng.");
        }
        if (order.OrderDetail.Status != OrderStatus.Pending && 
            order.OrderDetail.Status != OrderStatus.Confirmed &&
            order.OrderDetail.Status != OrderStatus.WaitingCustomerDecision)
        {
            return HttpErrors.BadRequest("ORDER_CANNOT_CANCEL", "Đơn hàng đã được chế biến hoặc đang giao, không thể hủy.");
        }
        if (order.CustomerId != userId)
        {
            return HttpErrors.Forbidden("ORDER_FORBIDDEN", "Bạn không có quyền huỷ đơn hàng này");
        }

        return await ApplyTransitionAsync(
            order,
            OrderAction.CancelByCustomer,
            ActorRole.Customer,
            userId,
            reason,
            beforeSave: currentOrder =>
            {
                currentOrder.OrderDetail.CancelledByUserId = userId;
                currentOrder.OrderDetail.CancelReason = reason;
            },
            onSuccess: async currentOrder =>
            {
                var admins = await _userRepository.GetUsersByRoleAsync("admin");
                foreach (var admin in admins)
                {
                    await CreateRequiredNotificationAsync(admin.Id, new NotificationRequest
                    {
                        Title = "Khách hàng hủy đơn hàng",
                        Message = $"Khách hàng đã hủy đơn #{currentOrder.OrderCode}. Lý do: {reason}",
                        Type = (int)NotificationType.ORDER,
                        Link = "/admin/orders"
                    });
                }

                await NotifyRestaurantUsersAsync(currentOrder.RestaurantId, new NotificationRequest
                {
                    Title = "Khách hàng đã hủy đơn",
                    Message = $"Khách hàng đã hủy đơn #{currentOrder.OrderCode}. Lý do: {reason}",
                    Type = (int)NotificationType.ORDER,
                    Link = "/restaurant/orders"
                });
                await NotifyAssignedShipperAsync(
                    currentOrder,
                    "Đơn hàng đã bị hủy",
                    $"Đơn #{currentOrder.OrderCode} đã bị khách hàng hủy. Lý do: {reason}");
            },
            transactionalSideEffects: true);
    }
        public async Task<Result> RespondToProposalAsync(Guid customerId, Guid orderId, OrderProposalResponseDto request)
    {
        var order = await _orderRepository.GetOrderById(orderId);
        if (order == null || order.CustomerId != customerId)
            return HttpErrors.NotFound("ORDER_NOT_FOUND", "Không tìm thấy đơn hàng");

        if (request.IsAccepted)
        {
            // Nhánh 1: Khách đồng ý → Confirmed (hoặc Cancelled nếu hết món)
            var itemsToRemove = order.OrderItems.Where(oi => oi.IsRemoved).ToList();
            foreach (var item in itemsToRemove)
                order.OrderItems.Remove(item);

            if (!order.OrderItems.Any())
            {
                return await ApplyTransitionAsync(
                    order,
                    action: OrderAction.CancelByCustomer,
                    actor: ActorRole.Customer,
                    actorUserId: customerId,
                    historyNote: "Hủy tự động do không còn món nào khả dụng.",
                    beforeSave: currentOrder =>
                    {
                        currentOrder.OrderDetail.CancelledByUserId = customerId;
                        currentOrder.OrderDetail.CancelReason = "Đơn hàng không còn món nào khả dụng.";
                    },
                    onSuccess: async o =>
                    {
                        await NotifyRestaurantUsersAsync(o.RestaurantId, new NotificationRequest
                        {
                            Title = "Đơn hàng bị hủy",
                            Message = $"Đơn #{o.OrderCode} bị hủy do không còn món nào khả dụng.",
                            Type = (int)NotificationType.ORDER,
                            Link = "/restaurant/orders"
                        });
                    });
            }

            // Còn món → Confirmed
            order.Subtotal = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);
            order.TotalAmount = order.Subtotal + order.ShippingFee;

            return await ApplyTransitionAsync(
                order,
                action: OrderAction.AcceptProposal,
                actor: ActorRole.Customer,
                actorUserId: customerId,
                historyNote: request.Note ?? "Khách hàng đồng ý với thay đổi của nhà hàng.",
                onSuccess: async o =>
                {
                    await CreateRequiredNotificationAsync(o.CustomerId, new NotificationRequest
                    {
                        Title = "Đang tìm shipper cho đơn hàng",
                        Message = $"Đơn hàng #{o.OrderCode} đã được xác nhận. Đang tìm shipper trong lúc nhà hàng chuẩn bị món.",
                        Type = (int)NotificationType.DELIVERY,
                        Link = "/account"
                    });
                    await NotifyRestaurantUsersAsync(o.RestaurantId, new NotificationRequest
                    {
                        Title = "Khách hàng xác nhận đơn hàng",
                        Message = $"Khách hàng đã xác nhận thay đổi món trong đơn #{o.OrderCode}.",
                        Type = (int)NotificationType.ORDER,
                        Link = "/restaurant/orders"
                    });
                    await NotifyDispatchShippersAsync(o);
                },
                transactionalSideEffects: true);
        }
        else
        {
            var reason = string.IsNullOrWhiteSpace(request.Note)
                ? "Khách hàng hủy đơn vì không đồng ý với thay đổi món của nhà hàng."
                : request.Note.Trim();
            return await ApplyTransitionAsync(
                order,
                action: OrderAction.RejectProposal,
                actor: ActorRole.Customer,
                actorUserId: customerId,
                historyNote: reason,
                beforeSave: currentOrder =>
                {
                    currentOrder.OrderDetail.CancelledByUserId = customerId;
                    currentOrder.OrderDetail.CancelReason = reason;
                },
                onSuccess: async o =>
                {
                    await NotifyRestaurantUsersAsync(o.RestaurantId, new NotificationRequest
                    {
                        Title = "Khách hàng từ chối đề xuất",
                        Message = $"Khách hàng đã từ chối đề xuất cho đơn #{o.OrderCode}.",
                        Type = (int)NotificationType.ORDER,
                        Link = "/restaurant/orders"
                    });
                });
        }
    }

    public async Task<Result<PagedResponse<OrderAdminSummaryResponse>>> GetOrderAdminAsync(OrderFilterModel filter)
    {
        var query = _orderRepository.GetQueryable();
        if (filter.Status.HasValue)
        {
            query = query.Where(o=>o.OrderDetail.Status == filter.Status.Value);
        }
        if (!string.IsNullOrWhiteSpace(filter.SearchCode))
        {
            var search = $"%{filter.SearchCode.Trim()}%";
            query = query.Where(o =>
            EF.Functions.ILike(o.OrderCode, search) ||
            EF.Functions.ILike(o.Customer.FullName, search)
            );
        }
        if (filter.FromDate.HasValue)
        {
            var fromDateUtc = DateTime.SpecifyKind(filter.FromDate.Value, DateTimeKind.Utc);
            query = query.Where(o=>o.CreatedAt >= fromDateUtc);
        }
        if (filter.ToDate.HasValue)
        {
            var toDateUtc = DateTime.SpecifyKind(filter.ToDate.Value, DateTimeKind.Utc)
                            .AddDays(1).AddTicks(-1);
            query = query.Where(o=>o.CreatedAt <= toDateUtc);
        }
        int totalCount = await query.CountAsync();
        var pageSize = filter.PageSize > 100 ? 100 : filter.PageSize < 1 ? 10 : filter.PageSize;
        var page = filter.Page < 1 ? 1 : filter.Page;
        var items = await query
            .OrderByDescending(o=>o.CreatedAt)
            .Skip((page -1 ) *pageSize)
            .Take(pageSize)
            .Select(o=> new OrderAdminSummaryResponse
            {
                Id = o.Id,
                OrderCode = o.OrderCode,
                CustomerName = o.Customer.FullName,
                TotalAmount = o.TotalAmount,
                Status = o.OrderDetail.Status,
                CreatedAt = o.CreatedAt,
                PaymentMethod = o.OrderDetail.PaymentMethod
            }).ToListAsync();
        var responseMeta = new PaginationMeta(
            Page: page,
            PageSize: pageSize,
            TotalCount: totalCount,
            TotalPages: (int)Math.Ceiling((double)totalCount / pageSize)
        );   
        var pageResponse = new PagedResponse<OrderAdminSummaryResponse>(
            Data: items,
            Meta: responseMeta
        );  
        return Result<PagedResponse<OrderAdminSummaryResponse>>.Success(pageResponse);
    }
    public async Task<Result> ConfirmOrderAsync(Guid userId, Guid orderId, int? prepMinutes = null)
    {
        if (prepMinutes is <= 0)
            return HttpErrors.BadRequest("INVALID_PREP_MINUTES", "Thời gian chuẩn bị phải lớn hơn 0.");

        var order = await _orderRepository.GetOrderById(orderId);
        if(order == null)
        {
            return HttpErrors.NotFound("ORDER_NOT_FOUND", "Không tìm thấy đơn hàng.");
        }
        var actorRole = await ResolveActorRoleAsync(userId);
        if(actorRole is null)
        {
            return HttpErrors.Forbidden("FORBIDDEN", "Không xác định được vai trò.");
        }
        if (actorRole.Value.IsRestaurantSide()
            && !await IsUserInRestaurantAsync(userId, order.RestaurantId, actorRole.Value))
        {
            return HttpErrors.Forbidden("FORBIDDEN", "Bạn không có quyền thực hiện hành động này.");
        }
        return await ApplyTransitionAsync(
            order,
            action: OrderAction.Confirm,
            actor: actorRole.Value,
            actorUserId: userId,
            historyNote: "Nhà hàng đã xác nhận đơn hàng.",
            beforeSave: currentOrder =>
            {
                if (prepMinutes.HasValue)
                    currentOrder.OrderDetail.PrepMinutes = prepMinutes.Value;
            },
            onSuccess: async o =>
            {
                await CreateRequiredNotificationAsync(o.CustomerId, new NotificationRequest
                {
                    Title = "Đơn hàng được xác nhận",
                    Message = $"Đơn hàng #{o.OrderCode} đã được xác nhận. Đang tìm shipper trong lúc nhà hàng chuẩn bị món.",
                    Type = (int)NotificationType.ORDER,
                    Link = "/account"
                });
                await NotifyDispatchShippersAsync(o);
            },
            transactionalSideEffects: true
        );
    }
    public async Task<Result> OutOfStockAsync(Guid adminId, Guid orderId, OutOfStockRequest request)
    {
        var order = await _orderRepository.GetOrderById(orderId);
        if(order == null)
        { 
            return HttpErrors.NotFound("ORDER_NOT_FOUND", "Không tìm thấy đơn hàng.");
        }
        if (order.OrderDetail.Status != OrderStatus.Pending)
            return HttpErrors.BadRequest("INVALID_STATUS", $"Không thể báo hết món ở trạng thái {order.OrderDetail.Status}.");

        var requestedIds = request.RemovedProductIds?.Where(id => id != Guid.Empty).ToHashSet()
            ?? new HashSet<Guid>();
        if (requestedIds.Count == 0)
            return HttpErrors.BadRequest("NO_ITEMS_SELECTED", "Hãy chọn ít nhất một món đã hết hàng.");

        var itemsToRemove = order.OrderItems
            .Where(item => !item.IsRemoved && requestedIds.Contains(item.ProductId))
            .ToList();
        if (!requestedIds.IsSubsetOf(itemsToRemove.Select(item => item.ProductId).ToHashSet()))
            return HttpErrors.BadRequest("INVALID_ORDER_ITEMS", "Danh sách món không hợp lệ với đơn hàng này.");

        var removedNames = string.Join(", ", itemsToRemove.Select(item => item.ProductName).Distinct());
        var reason = string.IsNullOrWhiteSpace(request.Note)
            ? $"Admin báo hết món: {removedNames}."
            : request.Note.Trim();
        var remainingItems = order.OrderItems
            .Where(item => !item.IsRemoved && !requestedIds.Contains(item.ProductId))
            .ToList();

        if (remainingItems.Count == 0)
        {
            var cancelReason = $"Đơn hàng bị hủy vì tất cả món đều hết hàng ({removedNames}).";
            return await ApplyTransitionAsync(
                order,
                OrderAction.CancelByAdmin,
                ActorRole.Admin,
                adminId,
                cancelReason,
                beforeSave: currentOrder =>
                {
                    currentOrder.OrderDetail.CancelledByUserId = adminId;
                    currentOrder.OrderDetail.CancelReason = cancelReason;
                    foreach (var item in itemsToRemove)
                    {
                        item.IsRemoved = true;
                        item.RemoveReason = "Hết món";
                    }
                },
                onSuccess: async currentOrder =>
                {
                    await CreateRequiredNotificationAsync(currentOrder.CustomerId, new NotificationRequest
                    {
                        Title = "Đơn hàng đã bị hủy",
                        Message = $"Đơn hàng #{currentOrder.OrderCode} đã bị hủy vì tất cả món đều hết hàng.",
                        Type = (int)NotificationType.ORDER,
                        Link = "/account"
                    });
                    await NotifyRestaurantUsersAsync(currentOrder.RestaurantId, new NotificationRequest
                    {
                        Title = "Đơn hàng đã bị hủy",
                        Message = $"Đơn hàng #{currentOrder.OrderCode} đã bị admin hủy vì tất cả món đều hết hàng.",
                        Type = (int)NotificationType.ORDER,
                        Link = "/restaurant/orders"
                    });
                },
                transactionalSideEffects: true);
        }

        return await ApplyTransitionAsync(
            order,
            OrderAction.ReportOutOfStock,
            ActorRole.Admin,
            adminId,
            reason,
            beforeSave: currentOrder =>
            {
                foreach (var item in itemsToRemove)
                {
                    item.IsRemoved = true;
                    item.RemoveReason = reason;
                }
            },
            onSuccess: async currentOrder =>
            {
                await CreateRequiredNotificationAsync(currentOrder.CustomerId, new NotificationRequest
                {
                    Title = "Nhà hàng báo món hết hàng",
                    Message = $"{removedNames} hiện không có sẵn. Vui lòng xác nhận tiếp tục với các món còn lại hoặc hủy đơn.",
                    Type = (int)NotificationType.ORDER,
                    Link = "/account"
                });
            },
            transactionalSideEffects: true);
    }

    public async Task<Result> ReportOutOfStockByRestaurantAsync(
        Guid userId,
        Guid orderId,
        OutOfStockRequest request)
    {
        var order = await _orderRepository.GetOrderById(orderId);
        if (order == null)
            return HttpErrors.NotFound("ORDER_NOT_FOUND", "Không tìm thấy đơn hàng.");

        var actor = await ResolveActorRoleAsync(userId);
        if (actor is not (ActorRole.RestaurantOwner or ActorRole.RestaurantStaff))
            return HttpErrors.Forbidden("FORBIDDEN", "Chỉ nhà hàng mới có thể báo món hết hàng.");

        if (!await IsUserInRestaurantAsync(userId, order.RestaurantId, actor.Value))
            return HttpErrors.Forbidden("FORBIDDEN", "Bạn không thuộc nhà hàng của đơn hàng này.");

        var requestedIds = request.RemovedProductIds?.Where(id => id != Guid.Empty).ToHashSet();
        if (requestedIds == null || requestedIds.Count == 0)
            return HttpErrors.BadRequest("NO_ITEMS_SELECTED", "Hãy chọn ít nhất một món đã hết hàng.");

        var itemsToRemove = order.OrderItems
            .Where(item => !item.IsRemoved && requestedIds.Contains(item.ProductId))
            .ToList();
        var availableIds = itemsToRemove.Select(item => item.ProductId).ToHashSet();
        if (!requestedIds.IsSubsetOf(availableIds))
            return HttpErrors.BadRequest("INVALID_ORDER_ITEMS", "Danh sách món không hợp lệ với đơn hàng này.");

        var removedNames = string.Join(", ", itemsToRemove.Select(item => item.ProductName).Distinct());
        var note = string.IsNullOrWhiteSpace(request.Note)
            ? $"Nhà hàng báo hết món: {removedNames}."
            : request.Note.Trim();

        var remainingItems = order.OrderItems
            .Where(item => !item.IsRemoved && !requestedIds.Contains(item.ProductId))
            .ToList();

        // Nếu tất cả các món trong đơn đều hết -> Trực tiếp hủy đơn
        if (remainingItems.Count == 0)
        {
            var cancelReason = $"Nhà hàng hết toàn bộ món trong đơn ({removedNames}).";
            return await ApplyTransitionAsync(
                order,
                action: OrderAction.CancelByRestaurant,
                actor: actor.Value,
                actorUserId: userId,
                historyNote: cancelReason,
                beforeSave: currentOrder =>
                {
                    currentOrder.OrderDetail.CancelledByUserId = userId;
                    currentOrder.OrderDetail.CancelReason = cancelReason;
                    foreach (var item in itemsToRemove)
                    {
                        item.IsRemoved = true;
                        item.RemoveReason = "Hết món";
                    }
                },
                onSuccess: async currentOrder =>
                {
                    await CreateRequiredNotificationAsync(currentOrder.CustomerId, new NotificationRequest
                    {
                        Title = "Đơn hàng đã bị hủy",
                        Message = $"Đơn hàng #{currentOrder.OrderCode} đã bị nhà hàng hủy do hết toàn bộ món ({removedNames}).",
                        Type = (int)NotificationType.ORDER,
                        Link = "/account"
                    });
                    await NotifyAssignedShipperAsync(
                        currentOrder,
                        "Đơn hàng đã bị hủy",
                        $"Đơn #{currentOrder.OrderCode} đã bị nhà hàng hủy do hết toàn bộ món.");
                    var admins = await _userRepository.GetUsersByRoleAsync("admin");
                    foreach (var admin in admins)
                    {
                        await CreateRequiredNotificationAsync(admin.Id, new NotificationRequest
                        {
                            Title = "Nhà hàng hủy đơn do hết món",
                            Message = $"Đơn #{currentOrder.OrderCode} bị hủy vì toàn bộ món đã hết hàng.",
                            Type = (int)NotificationType.ORDER,
                            Link = "/admin/orders"
                        });
                    }
                },
                transactionalSideEffects: true);
        }

        return await ApplyTransitionAsync(
            order,
            action: OrderAction.ReportOutOfStock,
            actor: actor.Value,
            actorUserId: userId,
            historyNote: note,
            beforeSave: currentOrder =>
            {
                foreach (var item in itemsToRemove)
                {
                    item.IsRemoved = true;
                    item.RemoveReason = note;
                }
            },
            onSuccess: async currentOrder =>
            {
                await CreateRequiredNotificationAsync(currentOrder.CustomerId, new NotificationRequest
                {
                    Title = "Nhà hàng báo món hết hàng",
                    Message = $"{removedNames} hiện không có sẵn. Vui lòng xác nhận tiếp tục với các món còn lại hoặc hủy đơn.",
                    Type = (int)NotificationType.ORDER,
                    Link = "/account"
                });
            },
            transactionalSideEffects: true);
    }

    //chuyển trạng thái qua preparing
    public async Task<Result> StartPreparingAsync(Guid userId, Guid orderId)
    {
        var order = await _orderRepository.GetOrderById(orderId);
        if (order == null)
        {
            return HttpErrors.NotFound("ORDER_NOT_FOUND", "Không tìm thấy đơn hàng.");
        }
        var actorRole = await ResolveActorRoleAsync(userId);
        if (actorRole is null || !actorRole.Value.IsRestaurantSide())
        {
            return HttpErrors.Forbidden("FORBIDDEN", "Bạn không có quyền thực hiện hành động này.");
        }
        if (!await IsUserInRestaurantAsync(userId, order.RestaurantId, actorRole.Value))
        {
            return HttpErrors.Forbidden("FORBIDDEN", "Bạn không thuộc nhà hàng này.");            
        }
        return await ApplyTransitionAsync(
            order,
            action: OrderAction.StartPreparing,
            actor: actorRole.Value,
            actorUserId: userId,
            historyNote: "Nhà hàng bắt đầu chế biến món ăn.",
            onSuccess: async o =>
            {
                await _notificationService.CreateNotificationAsync(o.CustomerId, new NotificationRequest
                {
                    Title = "Đơn hàng đang được chế biến",
                    Message = $"Đơn hàng #{o.OrderCode} của bạn đang được nhà hàng chế biến. Vui lòng chờ...",
                    Type = (int)NotificationType.ORDER,
                    Link = "/account"
                });
            }  
        );
    }
    //Nấu xong và sẵn sang cho shipper lấy hàng
    public async Task<Result> MarkAsReadyAsync(Guid userId, Guid orderId)
    {
        var order = await _orderRepository.GetOrderById(orderId);
        if (order == null)
            return HttpErrors.NotFound("ORDER_NOT_FOUND", "Không tìm thấy đơn hàng.");

        var actorRole = await ResolveActorRoleAsync(userId);
        if (actorRole is null || !actorRole.Value.IsRestaurantSide())
            return HttpErrors.Forbidden("FORBIDDEN", "Chỉ nhà hàng mới được đánh dấu sẵn sàng.");

        if (!await IsUserInRestaurantAsync(userId, order.RestaurantId, actorRole.Value))
            return HttpErrors.Forbidden("FORBIDDEN", "Bạn không thuộc nhà hàng này.");

        return await ApplyTransitionAsync(
            order,
            action: OrderAction.MarkReady,
            actor: actorRole.Value,
            actorUserId: userId,
            historyNote: "Món ăn đã sẵn sàng. Chờ Shipper đến lấy.",
            onSuccess: async o =>
            {
                await CreateRequiredNotificationAsync(o.CustomerId, new NotificationRequest
                {
                    Title = "Đơn hàng sẵn sàng giao",
                    Message = $"Đơn hàng #{o.OrderCode} đã sẵn sàng để shipper đến lấy.",
                    Type = (int)NotificationType.DELIVERY,
                    Link = "/account"
                });

                await NotifyAssignedShipperAsync(
                    o,
                    "Món đã sẵn sàng",
                    $"Món của đơn #{o.OrderCode} đã sẵn sàng. Bạn có thể đến quán lấy hàng.");
            },
            transactionalSideEffects: true
        );
    }
     public async Task<Result> CancelOrderByAdminAsync(Guid adminId, Guid orderId, CancelOrderRequestDto request)
    {
         var reason = request.Reason?.Trim();
         if (string.IsNullOrWhiteSpace(reason) || reason.Length > 300)
             return HttpErrors.BadRequest("INVALID_CANCEL_REASON", "Vui lòng nhập lý do hủy từ 1 đến 300 ký tự.");

         var order = await _orderRepository.GetOrderById(orderId);
        if (order == null)
            return HttpErrors.NotFound("ORDER_NOT_FOUND", "Không tìm thấy đơn hàng.");

        return await ApplyTransitionAsync(
            order,
            action: OrderAction.CancelByAdmin,
            actor: ActorRole.Admin,
            actorUserId: adminId,
            historyNote: reason,
            beforeSave: o =>
            {
                o.OrderDetail.CancelledByUserId = adminId;
                o.OrderDetail.CancelReason = reason;
            },
            onSuccess: async o =>
            {
                await CreateRequiredNotificationAsync(o.CustomerId, new NotificationRequest
                {
                    Title = "Đơn hàng đã bị hủy",
                    Message = $"Đơn hàng #{o.OrderCode} đã bị hủy. Lý do: {reason}",
                    Type = (int)NotificationType.ORDER,
                    Link = "/account"
                });

                await NotifyAssignedShipperAsync(
                    o,
                    "Đơn hàng đã bị hủy",
                    $"Đơn #{o.OrderCode} đã bị hủy. Lý do: {reason}");

                await NotifyRestaurantUsersAsync(o.RestaurantId, new NotificationRequest
                {
                    Title = "Đơn hàng đã bị hủy",
                    Message = $"Đơn hàng #{o.OrderCode} đã bị hủy bởi admin.",
                    Type = (int)NotificationType.ORDER,
                    Link = "/restaurant/orders"
                });
            },
            transactionalSideEffects: true);
    }

    public async Task<Result> CancelOrderByRestaurantAsync(Guid userId, Guid orderId, CancelOrderRequestDto request)
    {
        var reason = request.Reason?.Trim();
        if (string.IsNullOrWhiteSpace(reason) || reason.Length > 300)
            return HttpErrors.BadRequest("INVALID_CANCEL_REASON", "Vui lòng nhập lý do hủy từ 1 đến 300 ký tự.");

        var order = await _orderRepository.GetOrderById(orderId);
        if (order == null)
            return HttpErrors.NotFound("ORDER_NOT_FOUND", "Không tìm thấy đơn hàng.");

        var actorRole = await ResolveActorRoleAsync(userId);
        if (actorRole is null || !actorRole.Value.IsRestaurantSide())
            return HttpErrors.Forbidden("FORBIDDEN", "Chỉ nhà hàng mới được hủy đơn.");

        if (!await IsUserInRestaurantAsync(userId, order.RestaurantId, actorRole.Value))
            return HttpErrors.Forbidden("FORBIDDEN", "Bạn không thuộc nhà hàng này.");

        return await ApplyTransitionAsync(
            order,
            action: OrderAction.CancelByRestaurant,
            actor: actorRole.Value,
            actorUserId: userId,
            historyNote: reason,
            beforeSave: o =>
            {
                o.OrderDetail.CancelledByUserId = userId;
                o.OrderDetail.CancelReason = reason;
            },
            onSuccess: async o =>
            {
                await CreateRequiredNotificationAsync(o.CustomerId, new NotificationRequest
                {
                    Title = "Đơn hàng đã bị hủy",
                    Message = $"Đơn hàng #{o.OrderCode} đã bị hủy bởi nhà hàng. Lý do: {reason}",
                    Type = (int)NotificationType.ORDER,
                    Link = "/account"
                });

                await NotifyAssignedShipperAsync(
                    o,
                    "Đơn hàng đã bị hủy",
                    $"Đơn #{o.OrderCode} đã bị nhà hàng hủy. Lý do: {reason}");

                var admins = await _userRepository.GetUsersByRoleAsync("admin");
                foreach (var admin in admins)
                {
                    await CreateRequiredNotificationAsync(admin.Id, new NotificationRequest
                    {
                        Title = "Nhà hàng hủy đơn",
                        Message = $"Nhà hàng đã hủy đơn #{o.OrderCode}. Lý do: {reason}",
                        Type = (int)NotificationType.ORDER,
                        Link = "/admin/orders"
                    });
                }
            },
            transactionalSideEffects: true);
    }
    /// <summary>
    /// Mark order as delivered by shipper
    /// </summary>
    public async Task<Result> MarkAsDeliveredAsync(Guid shipperId, Guid orderId)
    {
        var order = await _orderRepository.GetOrderById(orderId);
        if (order == null)
        {
            return HttpErrors.NotFound("ORDER_NOT_FOUND", "Không tìm thấy đơn hàng.");
        }
        if (order.OrderDetail.Status != OrderStatus.Shipping)
        {
            return HttpErrors.BadRequest("INVALID_STATUS", "Đơn hàng không ở trạng thái đang giao.");
        }
        var shipper = await _shipperRepository.GetShipperByIdAsync(shipperId);
        if (shipper == null || order.OrderDetail.ShipperId != shipper.Id)
        {
            return HttpErrors.Forbidden("FORBIDDEN", "Bạn không được phân công giao đơn hàng này.");
        }

        return await ApplyTransitionAsync(
            order,
            OrderAction.Deliver,
            ActorRole.Shipper,
            shipperId,
            "Shipper đã giao thành công đơn hàng.",
            beforeSave: currentOrder =>
            {
                currentOrder.OrderDetail.ActualDeliveryTime = DateTime.UtcNow;
                shipper.TotalDeliveredOrders++;
            },
            onSuccess: async currentOrder =>
            {
                await CreateRequiredNotificationAsync(currentOrder.CustomerId, new NotificationRequest
                {
                    Title = "Đơn hàng đã được giao",
                    Message = $"Đơn hàng #{currentOrder.OrderCode} của bạn đã được giao thành công!",
                    Type = (int)NotificationType.DELIVERY,
                    Link = "/account"
                });
            },
            transactionalSideEffects: true);
    }

    /// <summary>
    /// Mark order payment as complete (when customer pays)
    /// </summary>
    public async Task<Result> ProcessVnPayPaymentAsync(
        Guid orderId,
        long paidAmount,
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(
            IsolationLevel.ReadCommitted,
            cancellationToken);
        var order = await _context.Orders
            .Include(currentOrder => currentOrder.OrderDetail)
            .FirstOrDefaultAsync(currentOrder => currentOrder.Id == orderId, cancellationToken);
        if (order == null)
            return HttpErrors.NotFound("ORDER_NOT_FOUND", "Không tìm thấy đơn hàng.");

        if (order.OrderDetail.PaymentMethod != PaymentMethod.VnPay)
            return HttpErrors.BadRequest("INVALID_PAYMENT_METHOD", "Đơn hàng không sử dụng VNPay.");

        var expectedAmount = decimal.Truncate(order.TotalAmount * 100m);
        if (order.TotalAmount <= 0 || expectedAmount != paidAmount)
            return HttpErrors.BadRequest("INVALID_PAYMENT_AMOUNT", "Số tiền thanh toán không khớp với đơn hàng.");

        if (order.OrderDetail.PaymentStatus is PaymentStatus.Paid or PaymentStatus.Refunded)
        {
            await transaction.CommitAsync();
            return Result.Success();
        }

        if (order.OrderDetail.PaymentStatus != PaymentStatus.Unpaid)
            return HttpErrors.Conflict("INVALID_PAYMENT_STATUS", "Trạng thái thanh toán không hợp lệ.");

        var wasCancelled = order.OrderDetail.Status == OrderStatus.Cancelled;
        order.OrderDetail.PaymentStatus = wasCancelled ? PaymentStatus.Refunded : PaymentStatus.Paid;
        if (wasCancelled)
        {
            await _walletService.CreditAsync(
                order.CustomerId,
                order.TotalAmount,
                WalletTransactionType.OrderRefund,
                order.Id,
                $"Hoàn tiền VNPay cho đơn {order.OrderCode} đã bị hủy trước khi thanh toán hoàn tất.",
                cancellationToken);
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            await transaction.RollbackAsync(cancellationToken);
            var currentStatus = await _context.OrderDetails
                .AsNoTracking()
                .Where(detail => detail.OrderId == orderId)
                .Select(detail => new { detail.PaymentStatus, detail.Status })
                .SingleOrDefaultAsync(cancellationToken);
            if (currentStatus?.PaymentStatus is PaymentStatus.Paid or PaymentStatus.Refunded)
                return Result.Success();
            return HttpErrors.Conflict("ORDER_CHANGED", "Đơn hàng vừa được cập nhật. VNPay sẽ gửi lại thông báo thanh toán.");
        }

        try
        {
            var admins = await _userRepository.GetUsersByRoleAsync("admin");
            foreach (var admin in admins)
            {
                var notificationRequest = new NotificationRequest
                {
                    Title = wasCancelled ? "Đã hoàn tiền vào ví khách hàng" : "Khách hàng đã thanh toán",
                    Message = wasCancelled
                        ? $"Đơn #{order.OrderCode} đã bị hủy. Tiền thanh toán VNPay được hoàn vào ví khách hàng."
                        : $"Khách hàng đã thanh toán đơn #{order.OrderCode}. Tổng tiền: {order.TotalAmount:N0} VND",
                    Type = (int)NotificationType.PAYMENT,
                    Link = "/admin/orders"
                };
                await _notificationService.CreateNotificationAsync(admin.Id, notificationRequest);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending notification: {ex.Message}");
        }

        await _notificationService.CreateNotificationAsync(order.CustomerId, new NotificationRequest
        {
            Title = wasCancelled ? "Tiền đã được hoàn vào ví" : "Thanh toán thành công",
            Message = wasCancelled
                ? $"Đơn #{order.OrderCode} đã bị hủy. Tiền thanh toán đã được hoàn vào ví FoodDelivery."
                : $"Thanh toán cho đơn #{order.OrderCode} đã được xác nhận.",
            Type = (int)NotificationType.PAYMENT,
            Link = "/account"
        });

        return Result.Success();
    }

    public async Task<Result> AssignShipperAsync(Guid adminId, Guid orderId, Guid shipperId)
    {
        var order = await _orderRepository.GetOrderById(orderId);
        if (order == null)
        {
            return HttpErrors.NotFound("ORDER_NOT_FOUND", "Không tìm thấy đơn hàng.");
        }

        if (order.OrderDetail.Status != OrderStatus.Confirmed
            && order.OrderDetail.Status != OrderStatus.Preparing
            && order.OrderDetail.Status != OrderStatus.ReadyForPickup)
        {
            return HttpErrors.BadRequest("INVALID_STATUS", "Chỉ có thể phân công shipper cho đơn đã được xác nhận.");
        }

        // Verify shipper exists and has appropriate role
        var shipperUser = await _userRepository.GetUserByIdWithRoleAsync(shipperId);
        var hasShipperRole = shipperUser?.UserRoles.Any(r => r.Role.Name == "Shipper") ?? false;
        if (!hasShipperRole)
        {
            return HttpErrors.BadRequest("INVALID_SHIPPER", "Người dùng này không phải là Shipper.");
        }

        // Get Shipper entity by UserId to get the correct Shipper.Id for FK
        var shipper = await _shipperRepository.GetShipperByIdAsync(shipperId);
        if (shipper == null)
        {
            return HttpErrors.NotFound("SHIPPER_NOT_FOUND", "Không tìm thấy hồ sơ shipper.");
        }
        if (order.CustomerId == shipper.UserId)
        {
            return HttpErrors.BadRequest(
                "SHIPPER_CANNOT_ACCEPT_OWN_ORDER",
                "Không thể phân công đơn hàng cho shipper đã đặt đơn này.");
        }
        if (shipper.Status != ShipperStatus.Approved || !shipper.IsActive || !shipper.IsAvailable)
            return HttpErrors.BadRequest("SHIPPER_NOT_AVAILABLE", "Shipper chưa được duyệt, đang tắt hoặc không nhận đơn.");
        if (!await _context.Users.AnyAsync(user => user.Id == shipperId && user.IsActive))
            return HttpErrors.BadRequest("SHIPPER_NOT_AVAILABLE", "Tài khoản shipper không hoạt động.");

        var assignedAt = DateTime.UtcNow;
        await using var transaction = await _context.Database.BeginTransactionAsync();
        var assigned = await _shipperRepository.TryAssignAvailableOrderAsync(orderId, shipper.Id, assignedAt);
        if (assigned == 0)
            return HttpErrors.BadRequest(
                "SHIPPER_BUSY_OR_ORDER_UNAVAILABLE",
                "Shipper đang có đơn cần xử lý hoặc đơn này không còn khả dụng.");

        order.OrderDetail.ShipperId = shipper.Id;
        order.OrderDetail.DispatchStatus = DispatchStatus.Assigned;
        order.OrderDetail.ShipperAssignedAt = assignedAt;
        await _orderRepository.AddOrderHistoryAsync(new OrderStatusHistory
        {
            OrderId = orderId,
            Status = order.OrderDetail.Status,
            ChangeByUserId = adminId,
            ChangedAt = assignedAt,
            ActionBy = OrderActionBy.Admin,
            Note = "Admin đã phân công shipper cho đơn hàng."
        });
        await _context.SaveChangesAsync();
        await CreateRequiredNotificationAsync(shipperId, new NotificationRequest
        {
            Title = "Đơn hàng được phân công",
            Message = $"Bạn được phân công giao đơn #{order.OrderCode}. Tổng tiền: {order.TotalAmount:N0} VND",
            Type = (int)NotificationType.DELIVERY,
            Link = "/shipper/orders"
        });
        await CreateRequiredNotificationAsync(order.CustomerId, new NotificationRequest
        {
            Title = "Shipper đã nhận đơn",
            Message = $"Đơn #{order.OrderCode} đã có shipper nhận giao.",
            Type = (int)NotificationType.DELIVERY,
            Link = "/account"
        });
        await NotifyRestaurantUsersAsync(order.RestaurantId, new NotificationRequest
        {
            Title = "Shipper đã nhận đơn",
            Message = $"Shipper đã nhận giao đơn #{order.OrderCode}.",
            Type = (int)NotificationType.DELIVERY,
            Link = "/restaurant/orders"
        });
        await transaction.CommitAsync();

        return Result.Success();
    }
}