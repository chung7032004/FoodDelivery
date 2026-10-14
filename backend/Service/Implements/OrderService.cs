using FoodDelivery.Common;
using FoodDelivery.DTOs.Order;
using FoodDelivery.DTOs.Notification;
using FoodDelivery.Entities;
using FoodDelivery.Repositories.Interfaces;
using FoodDelivery.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        FoodContext context)
    {
        _productRepository = productRepo;
        _orderRepository = orderRepo;
        _context = context;
        _cartItemRepository = cartItemRepo;
        _addressRepository = addressRepo;
        _restaurantRepository = restaurantRepository;
        _notificationService = notificationService;
        _userRepository = userRepository;
        _shipperRepository = shipperRepository;
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
            await _notificationService.CreateNotificationAsync(recipientId, request);
    }

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

            }
        };
        return await CreateOrderInternalAsync(customerId,request.AddressId, product.RestaurantId,item,request.Note);

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
        }).ToList();
        var result = await CreateOrderInternalAsync(customerId, request.AddressId, restaurantId, items, request.Note);
        if (result.IsSuccess)
        {
            await _cartItemRepository.DeleteRangeAsync(cartItems);
            await _context.SaveChangesAsync();
        }
        return result;
    }

    public async Task<Result<ShippingFeeResponseDto>> CalculateShippingFeeAsync(Guid addressId, Guid restaurantId)
    {
        var address = await _addressRepository.GetByIdAsync(addressId);
        if(address == null)
        {
            return HttpErrors.NotFound<ShippingFeeResponseDto>("ADDRESS_NOT_FOUND", "Không tìm thấy địa chỉ");
        }
        var restaurant = await _restaurantRepository.GetByIdAsync(restaurantId);
        if(restaurant == null)
        {
            return HttpErrors.BadRequest<ShippingFeeResponseDto>("RESTAURANT_NOT_CONFIGURED", "Cửa hàng chưa được cấu hình.");
        }
        
        var distance = CalculateDistance(restaurant.Latitude, restaurant.Longitude, address.Latitude, address.Longitude);
        
        if (distance > 40)
        {
            return HttpErrors.BadRequest<ShippingFeeResponseDto>("TOO_FAR", "Địa chỉ giao hàng quá xa (tối đa 40km)");
        }
        
        // Tính phí vận chuyển: Base 5,000 VND + 3,000 VND/km
        decimal shippingFee = 5000 + (decimal)distance * 3000;
        
        // Làm tròn đến 1000 VND gần nhất
        shippingFee = Math.Ceiling(shippingFee / 1000) * 1000;
        
        // Estimate delivery time: 10 mins base + 3 mins per km
        int estimatedMinutes = (int)(10 + (distance * 3));
        
        var response = new ShippingFeeResponseDto
        {
            ShippingFee = shippingFee,
            Distance = distance,
            EstimatedMinutes = estimatedMinutes
        };
        
        return Result<ShippingFeeResponseDto>.Success(response);
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
    private async Task<Result<CreateOrderResponseDto>> CreateOrderInternalAsync(
        Guid customerId, Guid addressId,
        Guid restaurantId,
        List<CreateOrderItemDto> items, string? note)
    {
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
        var distance = CalculateDistance(restaurant.Latitude,restaurant.Longitude,address.Latitude,address.Longitude);
        Console.WriteLine($"distance: {distance}");
        if (distance > 40)
        {
            return HttpErrors.BadRequest<CreateOrderResponseDto>("TOO_FAR", "Địa chỉ giao hàng quá xa (tối đa 40km)");
        }
        
        // Tính phí vận chuyển: Base 5,000 VND + 3,000 VND/km
        decimal shippingFee = 5000 + (decimal)distance * 3000;
        
        // Làm tròn đến 1000 VND gần nhất
        shippingFee = Math.Ceiling(shippingFee / 1000) * 1000;
        
        double estimated = 10 + (items.Count - 1) * 2 + (distance * 3);
        DateTime estimatedDeliveryTime = DateTime.UtcNow.AddMinutes(estimated);
        Console.WriteLine($"Shipping Fee: {shippingFee}, Distance: {distance}km");
        var order = new Order()
        {
            CustomerId = customerId,
            Note = note,
            RestaurantId = restaurantId,
            TotalAmount = items.Sum(i=>i.UnitPrice * i.Quantity),
            ShippingFee = shippingFee,
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
                ProductImage = item.ProductImage,
                ProductName = item.ProductName,
            });
        }
        order.OrderDetail = new OrderDetail
        {
            OrderId = order.Id,
            Status = OrderStatus.Pending,
            PaymentMethod = PaymentMethod.Cash,
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
            TotalAmount = order.TotalAmount,
            ShippingFee = shippingFee,
            EstimatedDeliveryTime = estimatedDeliveryTime
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
        foreach(var history in order.OrderStatusHistories)
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

        var orderStatusHistory = new OrderStatusHistory
        {
            Status = OrderStatus.Cancelled,
            OrderId = orderId,
            ChangeByUserId = userId,
            ChangedAt = DateTime.UtcNow,
            Note = request.Reason,
            ActionBy = OrderActionBy.Customer
        };
        order.OrderDetail.Status = OrderStatus.Cancelled;
        order.OrderDetail.CancelledByUserId = userId;
        order.OrderDetail.CancelReason = request.Reason;
        await _orderRepository.AddOrderHistoryAsync(orderStatusHistory);
        await _context.SaveChangesAsync();

        // 📢 Send notification to all admins: Customer cancelled order
        try
        {
            var admins = await _userRepository.GetUsersByRoleAsync("admin");
            foreach (var admin in admins)
            {
                var notificationRequest = new NotificationRequest
                {
                    Title = "Khách hàng hủy đơn hàng",
                    Message = $"Khách hàng đã hủy đơn #{order.OrderCode}. Lý do: {request.Reason ?? "Không có"}",
                    Type = (int)NotificationType.ORDER,
                    Link = "/admin/orders"
                };
                await _notificationService.CreateNotificationAsync(admin.Id, notificationRequest);
            }

            await NotifyRestaurantUsersAsync(order.RestaurantId, new NotificationRequest
            {
                Title = "Khách hàng đã hủy đơn",
                Message = $"Khách hàng đã hủy đơn #{order.OrderCode}. Lý do: {request.Reason ?? "Không có"}",
                Type = (int)NotificationType.ORDER,
                Link = "/restaurant/orders"
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending notification: {ex.Message}");
        }

        // Nếu khách đã thanh toán qua ví/thẻ -> Gọi Service hoàn tiền.
        return Result.Success();
    }
    public async Task<Result> RespondToProposalAsync(Guid customerId, Guid orderId, OrderProposalResponseDto request)
    {
        var order = await _orderRepository.GetOrderById(orderId);
        if(order == null || order.CustomerId != customerId)
        {
            return HttpErrors.NotFound("ORDER_NOT_FOUND", "Không tìm thấy đơn hàng");
        }
        if(order.OrderDetail.Status != OrderStatus.WaitingCustomerDecision)
        {
            return HttpErrors.BadRequest("INVALID_STATUS", "Đơn hàng không ở trạng thái chờ xác nhận");
        }
        // bỏ món
        var orderStatusHistory = new OrderStatusHistory
        {
            OrderId = orderId,
            ChangeByUserId = customerId,
            ChangedAt = DateTime.UtcNow,
            Note = request.Note ?? string.Empty,
            ActionBy = OrderActionBy.Customer
        };
        if (request.IsAccepted)
        {
            var itemsToRemove = order.OrderItems.Where(oi=> oi.IsRemoved).ToList();
            foreach(var item in itemsToRemove)
            {
                order.OrderItems.Remove(item);
            }
            if (!order.OrderItems.Any())
            {
                order.OrderDetail.Status = OrderStatus.Cancelled;
                orderStatusHistory.Status = OrderStatus.Cancelled;
                orderStatusHistory.ActionBy = OrderActionBy.System;
                orderStatusHistory.Note = "Hủy tự động do không còn món nào khả dụng.";
            }
            else
            {
                orderStatusHistory.Status = OrderStatus.Confirmed;
                order.OrderDetail.Status = OrderStatus.Confirmed;
                orderStatusHistory.ActionBy = OrderActionBy.Customer;
                order.TotalAmount = order.OrderItems.Sum(oi=> oi.Quantity*oi.UnitPrice);
            }    
        }
        else
        {
            order.OrderDetail.Status = OrderStatus.Cancelled;
            orderStatusHistory.Status = OrderStatus.Cancelled;
            orderStatusHistory.ActionBy = OrderActionBy.Customer;
            orderStatusHistory.Note = request.Note ?? "Khách hàng không đồng ý với thay đổi từ nhà hàng.";
        }
       try 
        {
            await _orderRepository.AddOrderHistoryAsync(orderStatusHistory);
            await _context.SaveChangesAsync();

           try
           {
               var notificationRequest = new NotificationRequest
               {
                   Title = request.IsAccepted ? "Khách hàng xác nhận đơn hàng" : "Khách hàng từ chối đề xuất",
                   Message = request.IsAccepted
                       ? $"Khách hàng đã xác nhận thay đổi món trong đơn #{order.OrderCode}."
                       : $"Khách hàng đã từ chối đề xuất cho đơn #{order.OrderCode}.",
                   Type = (int)NotificationType.ORDER,
                   Link = "/restaurant/orders"
               };
               await NotifyRestaurantUsersAsync(order.RestaurantId, notificationRequest);
           }
           catch (Exception notificationException)
           {
               Console.Error.WriteLine($"Failed to notify restaurant staff about customer response: {notificationException}");
           }

           return Result.Success();
       }
        catch (Exception ex)
        {
            return HttpErrors.InternalServerError("DB_ERROR", "Lỗi lưu dữ liệu: " + ex.Message);
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
    public async Task<Result> ConfirmOrderAsync(Guid adminId, Guid orderId)
    {
        var order = await _orderRepository.GetOrderById(orderId);
        if(order == null)
        {
            return HttpErrors.NotFound("ORDER_NOT_FOUND", "Không tìm thấy đơn hàng.");
        }
        if(order.OrderDetail.Status != OrderStatus.Pending)
        {
            return HttpErrors.BadRequest("INVALID_STATUS", $"Không thể xác nhận đơn hàng ở trạng thái {order.OrderDetail.Status}");
        }
        order.OrderDetail.Status = OrderStatus.Confirmed;
        var history = new OrderStatusHistory
        {
            ActionBy = OrderActionBy.Admin,
            ChangeByUserId = adminId,
            ChangedAt = DateTime.UtcNow,
            Note = "Admin đã xác nhận đơn hàng.",
            OrderId = orderId,
            Status = OrderStatus.Confirmed
        };

        await _orderRepository.AddOrderHistoryAsync(history);
        await _context.SaveChangesAsync();

        // 📢 Send notification to customer: Order confirmed
        try
        {
            var notificationRequest = new NotificationRequest
            {
                Title = "Đơn hàng được xác nhận",
                Message = $"Đơn hàng #{order.OrderCode} của bạn đã được nhà hàng xác nhận và đang chuẩn bị.",
                Type = (int)NotificationType.ORDER,
                Link = "/account"
            };
            await _notificationService.CreateNotificationAsync(order.CustomerId, notificationRequest);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending notification: {ex.Message}");
        }

        return Result.Success();
    }
    public async Task<Result> OutOfStockAsync(Guid adminId, Guid orderId, OutOfStockRequest request)
    {
        var order = await _orderRepository.GetOrderById(orderId);
        if(order == null)
        { 
            return HttpErrors.NotFound("ORDER_NOT_FOUND", "Không tìm thấy đơn hàng.");
        }
        if(order.OrderDetail.Status != OrderStatus.Pending)
        {
            return HttpErrors.BadRequest("INVALID_STATUS", $"Không thể xác nhận đơn hàng ở trạng thái {order.OrderDetail.Status}");
        }
        order.OrderDetail.Status = OrderStatus.WaitingCustomerDecision;
        var itemsToUpdate = order.OrderItems.Where(oi=> request.RemovedProductIds.Contains(oi.ProductId)).ToList();
        if (!itemsToUpdate.Any())
        {
            return HttpErrors.NotFound("NO_ITEMS_FOUND", "Không tìm thấy món ăn nào khớp với danh sách yêu cầu.");
        }
        foreach (var item in itemsToUpdate)
        {
            item.IsRemoved = true;
            item.RemoveReason = "Món ăn thiếu nguyên liệu để làm.";
        }
        var history = new OrderStatusHistory
        {
            ActionBy = OrderActionBy.Admin,
            ChangeByUserId = adminId,
            ChangedAt = DateTime.UtcNow,
            Note = request.Note ?? "Một số món ăn đã hết hàng, chờ khách hàng quyết định.",
            OrderId = orderId,
            Status = OrderStatus.WaitingCustomerDecision
        };
        await _orderRepository.AddOrderHistoryAsync(history);
        await _context.SaveChangesAsync();

        // 📢 Send notification to customer: Some items out of stock
        try
        {
            var removedItems = string.Join(", ", itemsToUpdate.Select(i => i.ProductName));
            var notificationRequest = new NotificationRequest
            {
                Title = "Một số món hàng hết stock",
                Message = $"Các món: {removedItems} không còn nguyên liệu. Vui lòng xác nhận tiếp tục hay hủy đơn.",
                Type = (int)NotificationType.ORDER,
                Link = "/account"
            };
            await _notificationService.CreateNotificationAsync(order.CustomerId, notificationRequest);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending notification: {ex.Message}");
        }

        return Result.Success(); 
    }
    //chuyển trạng thái qua preparing
    public async Task<Result> StartPreparingAsync(Guid adminId, Guid orderId)
    {
        var order = await _orderRepository.GetOrderById(orderId);
        if (order == null)
        {
            return HttpErrors.NotFound("ORDER_NOT_FOUND", "Không tìm thấy đơn hàng.");
        }
        if (order.OrderDetail.Status != OrderStatus.Confirmed)
        {
            return HttpErrors.BadRequest("INVALID_STATUS", $"Không thể nấu đơn hàng ở trạng thái {order.OrderDetail.Status}");
        }
        order.OrderDetail.Status = OrderStatus.Preparing;
        var history = new OrderStatusHistory
        {
            ActionBy = OrderActionBy.Admin,
            ChangeByUserId = adminId,
            ChangedAt = DateTime.UtcNow,
            Note = "Nhà hàng bắt đầu chế biến món ăn.",
            OrderId = orderId,
            Status = OrderStatus.Preparing
        };
        await _orderRepository.AddOrderHistoryAsync(history);
        await _context.SaveChangesAsync();

        // 📢 Send notification to customer: Order is being prepared
        try
        {
            var notificationRequest = new NotificationRequest
            {
                Title = "Đơn hàng đang được chế biến",
                Message = $"Đơn hàng #{order.OrderCode} đang được nhà hàng chế biến. Vui lòng chờ...",
                Type = (int)NotificationType.ORDER,
                Link = "/account"
            };
            await _notificationService.CreateNotificationAsync(order.CustomerId, notificationRequest);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending notification: {ex.Message}");
        }

        //Kích hoạt hệ thống tìm Shipper
        return Result.Success();
    }
    //Nấu xong và sẵn sang cho shipper lấy hàng
    public async Task<Result> MarkAsReadyAsync(Guid adminId, Guid orderId)
    {
        var order = await _orderRepository.GetOrderById(orderId);
        if (order == null)
        {
            return HttpErrors.NotFound("ORDER_NOT_FOUND", "Không tìm thấy đơn hàng.");
        }
        if (order.OrderDetail.Status != OrderStatus.Preparing)
        {
            return HttpErrors.BadRequest("INVALID_STATUS", "Đơn hàng chưa được nấu xong hoặc đang ở trạng thái khác.");
        }
        order.OrderDetail.Status = OrderStatus.ReadyForPickup;
        
        // 🚚 Auto-assign an available shipper
        var availableShipperId = await FindAvailableShipperAsync();
        Guid? shipperUserId = null;
        if (availableShipperId.HasValue)
        {
            order.OrderDetail.ShipperId = availableShipperId.Value;
            // Get the Shipper entity to retrieve UserId for notification
            var shipper = await _context.Shippers
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == availableShipperId.Value);
            shipperUserId = shipper?.UserId;
        }

        var history = new OrderStatusHistory
        {
            ActionBy = OrderActionBy.Admin,
            ChangeByUserId = adminId,
            ChangedAt = DateTime.UtcNow,
            Note = "Món ăn đã sẵn sàng. Chờ Shipper đến lấy.",
            OrderId = orderId,
            Status = OrderStatus.ReadyForPickup
        };
        await _orderRepository.AddOrderHistoryAsync(history);
        await _context.SaveChangesAsync();

        // 📢 Send notification to customer: Order ready for delivery
        try
        {
            var notificationRequest = new NotificationRequest
            {
                Title = "Đơn hàng sẵn sàng giao",
                Message = $"Đơn hàng #{order.OrderCode} của bạn đã sẵn sàng. Shipper sắp tới lấy hàng.",
                Type = (int)NotificationType.DELIVERY,
                Link = "/account"
            };
            await _notificationService.CreateNotificationAsync(order.CustomerId, notificationRequest);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending notification: {ex.Message}");
        }

        // 📢 Send notification to shipper if assigned
        if (shipperUserId.HasValue)
        {
            try
            {
                var notificationRequest = new NotificationRequest
                {
                    Title = "Đơn hàng được phân công",
                    Message = $"Bạn được phân công giao đơn #{order.OrderCode} cho {order.Customer?.FullName ?? "Khách hàng"}. Tổng tiền: {order.TotalAmount:N0} VND",
                    Type = (int)NotificationType.DELIVERY,
                    Link = "/shipper/orders"
                };
                await _notificationService.CreateNotificationAsync(shipperUserId.Value, notificationRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending notification to shipper: {ex.Message}");
            }
        }

        return Result.Success();
    }
    public async Task<Result> CancelOrderByAdminAsync(Guid adminId, Guid orderId, CancelOrderRequestDto request)
    {   
        var order = await _orderRepository.GetOrderById(orderId);
        if (order == null)
        {
            return HttpErrors.NotFound("ORDER_NOT_FOUND", "Không tìm thấy đơn hàng.");
        }
        if (order.OrderDetail.Status == OrderStatus.Completed || order.OrderDetail.Status == OrderStatus.Cancelled)
        {
            return HttpErrors.BadRequest("INVALID_STATUS", $"Không thể hủy đơn hàng đã {order.OrderDetail.Status}.");
        }
        var orderStatusHistory = new OrderStatusHistory
        {
            Status = OrderStatus.Cancelled,
            OrderId = orderId,
            ChangeByUserId = adminId,
            ChangedAt = DateTime.UtcNow,
            Note = request.Reason,
            ActionBy = OrderActionBy.Admin 
        };

        order.OrderDetail.Status = OrderStatus.Cancelled;
        order.OrderDetail.CancelledByUserId = adminId;
        order.OrderDetail.CancelReason = request.Reason;
        await _orderRepository.AddOrderHistoryAsync(orderStatusHistory);
        await _context.SaveChangesAsync();

        try
        {
            await _notificationService.CreateNotificationAsync(order.CustomerId, new NotificationRequest
            {
                Title = "Đơn hàng đã bị hủy",
                Message = $"Đơn hàng #{order.OrderCode} đã bị hủy. Lý do: {request.Reason ?? "Không có"}",
                Type = (int)NotificationType.ORDER,
                Link = "/account"
            });

            if (order.OrderDetail.ShipperId.HasValue)
            {
                var shipperUserId = await _context.Shippers
                    .Where(shipper => shipper.Id == order.OrderDetail.ShipperId.Value)
                    .Select(shipper => shipper.UserId)
                    .FirstOrDefaultAsync();
                if (shipperUserId != Guid.Empty)
                {
                    await _notificationService.CreateNotificationAsync(shipperUserId, new NotificationRequest
                    {
                        Title = "Đơn hàng đã bị hủy",
                        Message = $"Đơn #{order.OrderCode} đã bị hủy. Bạn không cần đến quán để lấy hàng.",
                        Type = (int)NotificationType.DELIVERY,
                        Link = "/shipper/orders"
                    });
                }
            }
        }
        catch (Exception notificationException)
        {
            Console.Error.WriteLine($"Failed to notify users about order cancellation: {notificationException}");
        }

        try
        {
            await NotifyRestaurantUsersAsync(order.RestaurantId, new NotificationRequest
            {
                Title = "Đơn hàng đã bị hủy",
                Message = $"Đơn hàng #{order.OrderCode} đã bị hủy bởi admin.",
                Type = (int)NotificationType.ORDER,
                Link = "/restaurant/orders"
            });
        }
        catch (Exception notificationException)
        {
            Console.Error.WriteLine($"Failed to notify restaurant about order cancellation: {notificationException}");
        }

        return Result.Success();
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

        order.OrderDetail.Status = OrderStatus.Completed;
        order.OrderDetail.ActualDeliveryTime = DateTime.UtcNow;

        var history = new OrderStatusHistory
        {
            ActionBy = OrderActionBy.Shipper,
            ChangeByUserId = shipperId,
            ChangedAt = DateTime.UtcNow,
            Note = "Shipper đã giao thành công đơn hàng.",
            OrderId = orderId,
            Status = OrderStatus.Completed
        };

        await _orderRepository.AddOrderHistoryAsync(history);
        await _context.SaveChangesAsync();

        // 📢 Send notification to customer: Order delivered
        try
        {
            var notificationRequest = new NotificationRequest
            {
                Title = "Đơn hàng đã được giao",
                Message = $"Đơn hàng #{order.OrderCode} của bạn đã được giao thành công!",
                Type = (int)NotificationType.DELIVERY,
                Link = "/account"
            };
            await _notificationService.CreateNotificationAsync(order.CustomerId, notificationRequest);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending notification: {ex.Message}");
        }

        return Result.Success();
    }

    /// <summary>
    /// Mark order payment as complete (when customer pays)
    /// </summary>
    public async Task<Result> MarkPaymentCompleteAsync(Guid orderId)
    {
        var order = await _orderRepository.GetOrderById(orderId);
        if (order == null)
        {
            return HttpErrors.NotFound("ORDER_NOT_FOUND", "Không tìm thấy đơn hàng.");
        }

        order.OrderDetail.PaymentStatus = PaymentStatus.Paid;

        await _context.SaveChangesAsync();

        // 📢 Send notification to all admins: Payment received
        try
        {
            var admins = await _userRepository.GetUsersByRoleAsync("admin");
            foreach (var admin in admins)
            {
                var notificationRequest = new NotificationRequest
                {
                    Title = "Khách hàng đã thanh toán",
                    Message = $"Khách hàng đã thanh toán đơn #{order.OrderCode}. Tổng tiền: {order.TotalAmount:N0} VND",
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
            Title = "Thanh toán thành công",
            Message = $"Thanh toán cho đơn #{order.OrderCode} đã được xác nhận.",
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

        if (order.OrderDetail.Status != OrderStatus.ReadyForPickup)
        {
            return HttpErrors.BadRequest("INVALID_STATUS", "Đơn hàng phải ở trạng thái 'Chờ Shipper' mới có thể phân công.");
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

        order.OrderDetail.ShipperId = shipper.Id;
        await _context.SaveChangesAsync();

        // 📢 Send notification to shipper: Order assigned
        try
        {
            var notificationRequest = new NotificationRequest
            {
                Title = "Đơn hàng được phân công",
                Message = $"Bạn được phân công giao đơn #{order.OrderCode} cho {order.Customer?.FullName ?? "Khách hàng"}. Tổng tiền: {order.TotalAmount:N0} VND",
                Type = (int)NotificationType.DELIVERY,
                Link = "/shipper/orders"
            };
            await _notificationService.CreateNotificationAsync(shipperId, notificationRequest);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending notification to shipper: {ex.Message}");
        }

        return Result.Success();
    }

    private async Task<Guid?> FindAvailableShipperAsync()
    {
        // Get all active shippers and return the first one
        // In a real implementation, you might want to consider shipper location, current load, etc.
        var shipper = await _context.Shippers
            .Include(s => s.User)
            .Where(s => s.IsActive && s.User.IsActive)
            .FirstOrDefaultAsync();
        return shipper?.Id;
    }            
    //tiệm đóng cửa
}