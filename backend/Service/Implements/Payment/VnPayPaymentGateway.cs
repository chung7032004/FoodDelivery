using FoodDelivery.Common;
using FoodDelivery.Entities;
using FoodDelivery.Service.Interfaces;
using FoodDelivery.Service.Interfaces.IPayment;
namespace FoodDelivery.Service.Implements.Payment;
public class VnPayPaymentGateway : IPaymentGateway
{
    private readonly IVnPayService _vnPayService;
    public VnPayPaymentGateway(IVnPayService vnPayService)
    {
        _vnPayService = vnPayService;
    }
    public PaymentMethod Method => PaymentMethod.VnPay;
    public Task<string?> CreatePaymentUrlAsync(Order order, HttpContext httpContext)
    {
        var url = _vnPayService.CreatePaymentUrl(order, httpContext);
        return Task.FromResult<string?>(url);
    }    
    public Task<Result> ProcessCallbackAsync(IQueryCollection query, CancellationToken ct = default)
    {
        var isValid = _vnPayService.ValidateCallback(query);
        if (!isValid)
        {
             return Task.FromResult(HttpErrors.BadRequest("INVALID_SIGNATURE", "Chữ ký VNPAY không hợp lệ."));
        }
        var responseCode = query["vnp_ResponseCode"].ToString();
        if (responseCode != "00")
        {
            return Task.FromResult(HttpErrors.BadRequest("PAYMENT_FAILED", "Thanh toán thất bại."));
        }
        return Task.FromResult(Result.Success());
    }
}