using FoodDelivery.Common;
using FoodDelivery.DTOs.Staff;
using FoodDelivery.DTOs.Order;
using FoodDelivery.DTOs.Review;
using FoodDelivery.Repositories.Interfaces;
using FoodDelivery.Service.Interfaces;

namespace FoodDelivery.Service.Implements;

public class StaffService : IStaffService
{
    private readonly IStaffRepository _staffRepository;
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IOrderService _orderService;

    public StaffService(
        IStaffRepository staffRepository,
        IRestaurantRepository restaurantRepository,
        IOrderService orderService)
    {
        _staffRepository = staffRepository;
        _restaurantRepository = restaurantRepository;
        _orderService = orderService;
    }

    private async Task<Guid?> GetRestaurantId(Guid userId)
    {
        var staff = await _staffRepository.GetStaffByUserId(userId);
        if (staff != null) return staff.RestaurantId;
        var restaurant = await _restaurantRepository.GetRestaurantByUserIdAsync(userId);
        return restaurant?.Id;
    }

    public async Task<Result<StaffStatsDto>> GetDashboardStats(Guid userId)
    {
        var restaurantId = await GetRestaurantId(userId);
        if (restaurantId == null)
            return Result<StaffStatsDto>.Failure("STAFF_NOT_FOUND", "Staff profile not found");

        var todayOrders = await _staffRepository.GetOrdersByRestaurant(restaurantId.Value);
        var todaysOrders = todayOrders.Where(o => o.CreatedAt.Date == DateTime.UtcNow.Date).ToList();
        
        var stats = new StaffStatsDto
        {
            TotalOrdersToday = todaysOrders.Count,
            PreparingCount = todaysOrders.Count(o => o.Status == 4),
            ReadyCount = todaysOrders.Count(o => o.Status == 5),
            WaitingCustomerDecisionCount = todaysOrders.Count(o => o.Status == 2),
            PendingCount = todaysOrders.Count(o => o.Status == 1),
            ConfirmedCount = todaysOrders.Count(o => o.Status == 3),
            ShippingCount = todaysOrders.Count(o => o.Status == 6)
        };

        return Result<StaffStatsDto>.Success(stats);
    }

    public async Task<Result<RestaurantAnalyticsDto>> GetRestaurantAnalytics(Guid userId, string period)
    {
        var restaurantId = await GetRestaurantId(userId);
        if (restaurantId == null)
            return Result<RestaurantAnalyticsDto>.Failure("STAFF_NOT_FOUND", "Không tìm thấy nhà hàng được gán cho tài khoản.");

        var today = DateTime.UtcNow.Date;
        DateTime from;
        DateTime to;
        int bucketCount;
        switch (period.ToLowerInvariant())
        {
            case "daily":
                bucketCount = 14;
                from = today.AddDays(1 - bucketCount);
                to = today.AddDays(1);
                break;
            case "monthly":
                bucketCount = 12;
                var thisMonth = new DateTime(today.Year, today.Month, 1);
                from = thisMonth.AddMonths(1 - bucketCount);
                to = thisMonth.AddMonths(1);
                break;
            case "yearly":
                bucketCount = 5;
                from = new DateTime(today.Year - bucketCount + 1, 1, 1);
                to = new DateTime(today.Year + 1, 1, 1);
                break;
            default:
                return Result<RestaurantAnalyticsDto>.Failure("INVALID_PERIOD", "Kỳ thống kê phải là daily, monthly hoặc yearly.");
        }

        var completedOrders = await _staffRepository.GetCompletedOrderAnalyticsAsync(restaurantId.Value, from, to);
        DateTime GetBucket(DateTime value) => period.ToLowerInvariant() switch
        {
            "daily" => value.Date,
            "monthly" => new DateTime(value.Year, value.Month, 1),
            _ => new DateTime(value.Year, 1, 1)
        };
        DateTime GetBucketAt(int index) => period.ToLowerInvariant() switch
        {
            "daily" => from.AddDays(index),
            "monthly" => from.AddMonths(index),
            _ => from.AddYears(index)
        };

        var grouped = completedOrders.GroupBy(order => GetBucket(order.CompletedAt))
            .ToDictionary(group => group.Key, group => new
            {
                Revenue = group.Sum(order => order.Revenue),
                CompletedOrders = group.Count(),
                OrdersMissingCost = group.Count(order => !order.GrossProfit.HasValue),
                GrossProfit = group.All(order => order.GrossProfit.HasValue)
                    ? group.Sum(order => order.GrossProfit!.Value)
                    : (decimal?)null
            });
        var points = Enumerable.Range(0, bucketCount).Select(index =>
        {
            var start = GetBucketAt(index);
            if (!grouped.TryGetValue(start, out var aggregate))
                return new RestaurantAnalyticsPointDto { PeriodStart = start, GrossProfit = 0 };
            return new RestaurantAnalyticsPointDto
            {
                PeriodStart = start,
                Revenue = aggregate.Revenue,
                GrossProfit = aggregate.GrossProfit,
                CompletedOrders = aggregate.CompletedOrders,
                OrdersMissingCost = aggregate.OrdersMissingCost
            };
        }).ToList();

        return Result<RestaurantAnalyticsDto>.Success(new RestaurantAnalyticsDto
        {
            Period = period.ToLowerInvariant(),
            Revenue = completedOrders.Sum(order => order.Revenue),
            GrossProfit = completedOrders.All(order => order.GrossProfit.HasValue)
                ? completedOrders.Sum(order => order.GrossProfit!.Value)
                : null,
            CompletedOrders = completedOrders.Count,
            OrdersMissingCost = completedOrders.Count(order => !order.GrossProfit.HasValue),
            Points = points
        });
    }

    public async Task<Result<List<OrderDto>>> GetOrdersForPreparation(Guid userId)
    {
        var restaurantId = await GetRestaurantId(userId);
        if (restaurantId == null)
            return Result<List<OrderDto>>.Failure("STAFF_NOT_FOUND", "Staff profile not found");

        var orders = await _staffRepository.GetPendingOrdersByRestaurant(restaurantId.Value);
        return Result<List<OrderDto>>.Success(orders);
    }

    public async Task<Result<List<OrderDto>>> GetCompletedOrders(Guid userId, int days = 30)
    {
        var restaurantId = await GetRestaurantId(userId);
        if (restaurantId == null)
            return Result<List<OrderDto>>.Failure("STAFF_NOT_FOUND", "Staff profile not found");

        var orders = await _staffRepository.GetCompletedOrdersByRestaurant(restaurantId.Value, days);
        return Result<List<OrderDto>>.Success(orders);
    }

    public async Task<Result<List<ReviewDto>>> GetRestaurantReviews(Guid userId, int? rating = null)
    {
        var restaurantId = await GetRestaurantId(userId);
        if (restaurantId == null)
            return Result<List<ReviewDto>>.Failure("STAFF_NOT_FOUND", "Staff profile not found");

        var reviews = await _staffRepository.GetRestaurantReviewsAsync(restaurantId.Value, rating);
        return Result<List<ReviewDto>>.Success(reviews);
    }

    public async Task<Result<OrderDto>> GetOrderDetails(Guid userId, Guid orderId)
    {
        var restaurantId = await GetRestaurantId(userId);
        if (restaurantId == null)
            return Result<OrderDto>.Failure("STAFF_NOT_FOUND", "Staff profile not found");

        var order = await _staffRepository.GetOrderDetails(orderId, restaurantId.Value);
        if (order == null)
            return Result<OrderDto>.Failure("ORDER_NOT_FOUND", "Order not found");

        return Result<OrderDto>.Success(order);
    }

    public async Task<Result<string>> UpdateOrderStatus(Guid userId, Guid orderId, int newStatus)
    {
        var result = newStatus switch
        {
            3 => await _orderService.ConfirmOrderAsync(userId, orderId),
            4 => await _orderService.StartPreparingAsync(userId, orderId),
            5 => await _orderService.MarkAsReadyAsync(userId, orderId),
            _ => Result.Failure("INVALID_STATUS", "Nhà hàng chỉ có thể chuyển đơn theo đúng thứ tự.")
        };
        return ToStatusResult(result, "Đã cập nhật trạng thái đơn hàng.");
    }

    public async Task<Result<StaffProfileDto>> GetStaffProfile(Guid userId)
    {
        var staff = await _staffRepository.GetStaffByUserId(userId);
        if (staff == null)
            return Result<StaffProfileDto>.Failure("STAFF_NOT_FOUND", "Staff profile not found");

        var profileDto = new StaffProfileDto
        {
            Id = staff.Id,
            UserId = staff.UserId,
            RestaurantId = staff.RestaurantId,
            Position = staff.Position,
            IsActive = staff.IsActive,
            RestaurantName = staff.Restaurant?.Name ?? "Unknown",
            UserName = staff.User?.FullName ?? "Unknown"
        };

        return Result<StaffProfileDto>.Success(profileDto);
    }

    public async Task<Result<string>> ConfirmOrder(Guid userId, Guid orderId, int? prepMinutes = null)
    {
        return ToStatusResult(
            await _orderService.ConfirmOrderAsync(userId, orderId, prepMinutes),
            "Đã xác nhận đơn hàng.");
    }

    public async Task<Result<string>> StartPreparingOrder(Guid userId, Guid orderId)
    {
        return ToStatusResult(
            await _orderService.StartPreparingAsync(userId, orderId),
            "Đã bắt đầu chế biến đơn hàng.");
    }

    public async Task<Result<string>> MarkOrderReady(Guid userId, Guid orderId)
    {
        return ToStatusResult(
            await _orderService.MarkAsReadyAsync(userId, orderId),
            "Đơn hàng đã sẵn sàng cho shipper.");
    }

    private static Result<string> ToStatusResult(Result result, string successMessage) =>
        result.IsSuccess
            ? Result<string>.Success(successMessage)
            : Result<string>.Failure(
                result.ErrorCode ?? "ORDER_UPDATE_FAILED",
                result.Message ?? "Không thể cập nhật trạng thái đơn hàng.",
                result.StatusCode);

    public async Task<Result<string>> ReportOutOfStock(
        Guid userId,
        Guid orderId,
        List<Guid> removedProductIds,
        string note)
    {
        var result = await _orderService.ReportOutOfStockByRestaurantAsync(userId, orderId, new OutOfStockRequest
        {
            RemovedProductIds = removedProductIds,
            Note = note
        });
        return ToStatusResult(result, "Đã báo hết món và gửi đề xuất cho khách hàng.");
    }

    public async Task<Result<string>> ReportOrderIssue(Guid userId, Guid orderId, string issueDescription, string issueType)
    {
        var restaurantId = await GetRestaurantId(userId);
        if (restaurantId == null)
            return Result<string>.Failure("STAFF_NOT_FOUND", "Staff profile not found");

        var success = await _staffRepository.ReportOrderIssueAsync(orderId, restaurantId.Value, issueDescription, issueType);
        if (!success)
            return Result<string>.Failure("REPORT_FAILED", "Failed to report issue");

        return Result<string>.Success("Issue reported to admin");
    }
}
