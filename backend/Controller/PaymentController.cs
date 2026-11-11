using FoodDelivery.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace FoodDelivery.Controller;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IVnPayService _vnPayService;
    private readonly IOrderService _orderService;

    public PaymentController(IVnPayService vnPayService, IOrderService orderService)
    {
        _vnPayService = vnPayService;
        _orderService = orderService;
    }

    /// <summary>
    /// IPN Webhook: VNPay Server gọi ngầm xác nhận giao dịch (Server-to-Server)
    /// </summary>
    [HttpGet("vnpay-ipn")]
    public async Task<IActionResult> VnPayIpn(CancellationToken cancellationToken)
    {
        if (!_vnPayService.ValidateCallback(Request.Query))
            return Ok(new { RspCode = "97", Message = "Invalid Signature" });

        if (!Guid.TryParse(Request.Query["vnp_TxnRef"], out var orderId))
            return Ok(new { RspCode = "01", Message = "Order Not Found" });

        if (!long.TryParse(Request.Query["vnp_Amount"], NumberStyles.None, CultureInfo.InvariantCulture, out var paidAmount))
            return Ok(new { RspCode = "04", Message = "Invalid Amount" });

        if (Request.Query["vnp_ResponseCode"] != "00"
            || Request.Query["vnp_TransactionStatus"] != "00")
            return Ok(new { RspCode = "02", Message = "Payment Failed" });

        var result = await _orderService.ProcessVnPayPaymentAsync(orderId, paidAmount, cancellationToken);
        if (result.IsSuccess)
            return Ok(new { RspCode = "00", Message = "Confirm Success" });

        var responseCode = result.ErrorCode switch
        {
            "ORDER_NOT_FOUND" => "01",
            "INVALID_PAYMENT_AMOUNT" or "INVALID_PAYMENT_METHOD" => "04",
            _ => "99"
        };
        return Ok(new { RspCode = responseCode, Message = result.Message ?? "Payment processing failed" });
    }

    /// <summary>
    /// Frontend gọi endpoint này để kiểm tra tính hợp lệ khi khách được chuyển hướng về trang web
    /// </summary>
    [HttpGet("vnpay-verify")]
    public async Task<IActionResult> VerifyPayment(CancellationToken cancellationToken)
    {
        if (!_vnPayService.ValidateCallback(Request.Query))
            return BadRequest(new { success = false, message = "Chữ ký không hợp lệ." });

        var orderIdStr = Request.Query["vnp_TxnRef"].ToString();
        if (!Guid.TryParse(orderIdStr, out var orderId))
            return BadRequest(new { success = false, message = "Không tìm thấy đơn hàng." });

        if (!long.TryParse(Request.Query["vnp_Amount"], NumberStyles.None, CultureInfo.InvariantCulture, out var paidAmount))
            return BadRequest(new { success = false, orderId, message = "Số tiền thanh toán không hợp lệ." });

        if (Request.Query["vnp_ResponseCode"] != "00"
            || Request.Query["vnp_TransactionStatus"] != "00")
            return BadRequest(new { success = false, orderId, message = "Thanh toán thất bại hoặc đã bị hủy." });

        var result = await _orderService.ProcessVnPayPaymentAsync(orderId, paidAmount, cancellationToken);
        if (result.IsSuccess)
            return Ok(new { success = true, orderId, message = "Thanh toán đã được xác nhận." });

        return BadRequest(new { success = false, orderId, message = result.Message ?? "Không thể xác nhận thanh toán." });
    }
}