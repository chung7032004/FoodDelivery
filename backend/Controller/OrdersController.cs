using System.Security.Claims;
using FoodDelivery.DTOs.Order;
using FoodDelivery.Extensions;
using FoodDelivery.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PermissionConstants = FoodDelivery.Constants.Permissions;
namespace FoodDelivery.Controllers;

[ApiController]
[Route("api/")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    [HttpPost("orders/buy-now")]
    [HasPermission(PermissionConstants.Orders.Create)]
    public async Task<IActionResult> CreateOrderBuyNow([FromBody] BuyNowRequestDto request)
    {
        var customerId = User.GetUserId();
        var result = await _orderService.BuyNowAsync(customerId,request);
        if (!result.IsSuccess)
        {
            return result.ErrorCode switch
            {
                "INVALID_QUANTITY" =>BadRequest(result),
                "PRODUCT_NOT_FOUND" => NotFound(result),
                "ADDRESS_NOT_FOUND" => NotFound(result),
                "PRODUCT_UNAVAILABLE" => BadRequest(result),
                "TOO_FAR" => BadRequest(result),
                "RESTAURANT_NOT_CONFIGURED" =>Conflict(result),
                _ => BadRequest(result),
            };
        }

        return CreatedAtAction(nameof(GetOrderDetail),new { orderId = result.Data!.OrderId},result);
    }

    [HttpPost("orders/checkout")]
    [HasPermission(PermissionConstants.Orders.Create)]
    public async Task<IActionResult> CheckoutCartItems([FromBody] CheckoutRequestDto request)
    {
        var customerId = User.GetUserId();
        var result = await _orderService.CheckoutAsync(customerId,request);
        if (!result.IsSuccess)
        {
            return result.ToActionResult();
        }
        return CreatedAtAction(nameof(GetOrderDetail),new { orderId = result.Data!.OrderId},result);
    }

    [HttpGet("orders/shipping-fee")]
    [HasPermission(PermissionConstants.Orders.View)]
    public async Task<IActionResult> CalculateShippingFee([FromQuery] Guid addressId, [FromQuery] Guid restaurantId)
    {
        var result = await _orderService.CalculateShippingFeeAsync(addressId,restaurantId);
        return result.ToActionResult();
    }

    [HttpGet("orders/history")]
    [HasPermission(PermissionConstants.Orders.ViewHistory)]
    public async Task<IActionResult> GetMyOrderHistory([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var customerId = User.GetUserId();
         var result = await _orderService.GetMyOrderAsync(customerId, page, pageSize);
        return result.ToActionResult();
    }
    [HttpGet("orders/{orderId}")]
    [HasPermission(PermissionConstants.Orders.View)]
    public async Task<IActionResult> GetOrderDetail(Guid orderId)
    {
        var customerId = User.GetUserId();
        var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
        Console.WriteLine($"[GetOrderDetail] UserId: {customerId}, Roles: {string.Join(",", roles)}");
        
        var result = await _orderService.GetOrderDetailAsync(customerId,roles,orderId);
        Console.WriteLine($"[GetOrderDetail] Service result - Success: {result.IsSuccess}, Error: {result.ErrorCode}");
        return result.ToActionResult();
    }
    [HttpPost("orders/{orderId}/cancel")]
    [HasPermission(PermissionConstants.Orders.Cancel)]
    public async Task<IActionResult> CancelOrderByCustomer(Guid orderId, [FromBody] CancelOrderRequestDto request)
    {
        var customerId = User.GetUserId();
        var result = await _orderService.CancelOrderByCustomerAsync(customerId,orderId,request);
        return result.ToActionResult();
    }
    [HttpPost("orders/{orderId}/respond-proposal")]
    [HasPermission(PermissionConstants.Orders.RespondToProposal)]
    public async Task<IActionResult> RespondToProposal(Guid orderId , [FromBody] OrderProposalResponseDto request)
    {
        var customerId = User.GetUserId();
        var result = await _orderService.RespondToProposalAsync(customerId,orderId,request);
        return result.ToActionResult();
    }
    
    [HttpGet("admin/orders")]
    [HasPermission(PermissionConstants.Orders.ViewAll)]
    public async Task<IActionResult> GetAdminOrder([FromQuery] OrderFilterModel filter)
    {
        var result = await _orderService.GetOrderAdminAsync(filter);
        return result.ToActionResult();
    }
    [HttpPost("admin/orders/{orderId}/confirm")]
    [HasPermission(PermissionConstants.Orders.ConfirmOrder)]
    public async Task<IActionResult> ConfirmOrder(Guid orderId)
    {
        var adminId = User.GetUserId();
        var result = await _orderService.ConfirmOrderAsync(adminId,orderId);
        return result.ToActionResult();
    }
    [HttpPost("admin/orders/{orderId}/out-of-stock")]
    [HasPermission(PermissionConstants.Orders.OutOfStock)]
    public async Task<IActionResult> OutOfStock(Guid orderId, [FromBody] OutOfStockRequest request)
    {
        var adminId = User.GetUserId();
        var result = await _orderService.OutOfStockAsync(adminId,orderId,request);
        return result.ToActionResult();
    }
    [HttpPost("admin/orders/{orderId}/start-preparing")]
    [HasPermission(PermissionConstants.Orders.StartPreparing)]
    public async Task<IActionResult> StartPreparing(Guid orderId)
    {
        var adminId = User.GetUserId();
        var result = await _orderService.StartPreparingAsync(adminId,orderId);
        return result.ToActionResult();
    }
    [HttpPost("admin/orders/{orderId}/final-preparing")]
    [HasPermission(PermissionConstants.Orders.MarkAsReady)]
    public async Task<IActionResult> MarkAsReady(Guid orderId)
    {
        var adminId = User.GetUserId();
        var result = await _orderService.MarkAsReadyAsync(adminId,orderId);
        return result.ToActionResult();
    }

    /// <summary>
    /// Mark order as delivered (Shipper)
    /// </summary>
    [HttpPut("shipper/orders/{orderId}/delivered")]
    [HasPermission(PermissionConstants.Orders.MarkAsDelivered)]
    public async Task<IActionResult> MarkAsDelivered(Guid orderId)
    {
        var shipperId = User.GetUserId();
        var result = await _orderService.MarkAsDeliveredAsync(shipperId, orderId);
        return result.ToActionResult();
    }
    
    [HttpPut("orders/{orderId}/payment-complete")]
    [HasPermission(PermissionConstants.Orders.MarkPaymentComplete)]
    public async Task<IActionResult> MarkPaymentComplete(Guid orderId)
    {
        var result = await _orderService.MarkPaymentCompleteAsync(orderId);
        return result.ToActionResult();
    }

    [HttpPost("admin/orders/{orderId}/cancel")]
    [HasPermission(PermissionConstants.Orders.Cancel)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CancelByAdmin(Guid orderId, [FromBody] CancelOrderRequestDto request)
    {
        var adminId = User.GetUserId();
        var result = await _orderService.CancelOrderByAdminAsync(adminId,orderId,request);
        return result.ToActionResult();
    }

    [HttpPost("admin/orders/{orderId}/assign-shipper")]
    [HasPermission(PermissionConstants.Orders.AssignShipper)]
    public async Task<IActionResult> AssignShipper(Guid orderId, [FromBody] AssignShipperRequestDto request)
    {
        var adminId = User.GetUserId();
        var result = await _orderService.AssignShipperAsync(adminId, orderId, request.ShipperId);
        return result.ToActionResult();
    }
}