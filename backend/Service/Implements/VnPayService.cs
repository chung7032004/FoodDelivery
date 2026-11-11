using FoodDelivery.Common.Options;
using FoodDelivery.Common.VnPay;
using FoodDelivery.Entities;
using FoodDelivery.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace FoodDelivery.Service.Implements;

public class VnPayService : IVnPayService
{
    private readonly VnpayOptions _options;

    public VnPayService(IOptions<VnpayOptions> options)
    {
        _options = options.Value;
    }

    public string CreatePaymentUrl(Order order, HttpContext httpContext)
    {
        if (string.IsNullOrWhiteSpace(_options.TmnCode)
            || string.IsNullOrWhiteSpace(_options.HashSecret)
            || string.IsNullOrWhiteSpace(_options.BaseUrl)
            || string.IsNullOrWhiteSpace(_options.ReturnUrl))
        {
            throw new InvalidOperationException(
                "VNPay must be configured using Vnpay__TmnCode, Vnpay__HashSecret, Vnpay__BaseUrl, and Vnpay__ReturnUrl.");
        }
        if (order.OrderDetail.Status == OrderStatus.Cancelled
            || order.OrderDetail.PaymentStatus != PaymentStatus.Unpaid)
            throw new InvalidOperationException("Only unpaid, active orders can create a VNPay payment URL.");
        if (order.TotalAmount <= 0 || decimal.Truncate(order.TotalAmount * 100m) != order.TotalAmount * 100m)
            throw new InvalidOperationException("VNPay order amount must be a positive whole number of VND.");

        var vnpay = new VnPayLibrary();
        var clientIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
        if (clientIp == "::1") clientIp = "127.0.0.1";

        vnpay.AddRequestData("vnp_Version", _options.Version);
        vnpay.AddRequestData("vnp_Command", _options.Command);
        vnpay.AddRequestData("vnp_TmnCode", _options.TmnCode);
        vnpay.AddRequestData("vnp_Amount", ((long)(order.TotalAmount * 100m)).ToString(System.Globalization.CultureInfo.InvariantCulture));
        vnpay.AddRequestData("vnp_CreateDate", DateTime.UtcNow.AddHours(7).ToString("yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture));
        vnpay.AddRequestData("vnp_CurrCode", _options.CurrCode);
        vnpay.AddRequestData("vnp_IpAddr", clientIp);
        vnpay.AddRequestData("vnp_Locale", _options.Locale);
        vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang {order.OrderCode}");
        vnpay.AddRequestData("vnp_OrderType", "other");
        vnpay.AddRequestData("vnp_ReturnUrl", _options.ReturnUrl);
        vnpay.AddRequestData("vnp_TxnRef", order.Id.ToString());

        return vnpay.CreateRequestUrl(_options.BaseUrl, _options.HashSecret);
    }

    public bool ValidateCallback(IQueryCollection query)
    {
        var vnpay = new VnPayLibrary();
        foreach (var (key, value) in query)
        {
            if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                vnpay.AddResponseData(key, value.ToString());
        }

        if (string.IsNullOrWhiteSpace(_options.TmnCode)
            || string.IsNullOrWhiteSpace(_options.HashSecret)
            || !string.Equals(
                query["vnp_TmnCode"].ToString(),
                _options.TmnCode,
                StringComparison.Ordinal))
        {
            return false;
        }

        var inputHash = query["vnp_SecureHash"].ToString();
        return vnpay.ValidateSignature(inputHash, _options.HashSecret);
    }
}