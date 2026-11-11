using FoodDelivery.Common;
using FoodDelivery.Entities;
using FoodDelivery.Service.Interfaces.IPayment;

namespace FoodDelivery.Service.Implements.Payment;
public class CashPaymentGateway : IPaymentGateway
{
    public PaymentMethod Method => PaymentMethod.Cash;
    public Task<string?> CreatePaymentUrlAsync(Order order, HttpContext httpContext)
    {
        return Task.FromResult<string?>(null);
    }
    public Task<Result> ProcessCallbackAsync(IQueryCollection query, CancellationToken ct = default)
    {
        return Task.FromResult(Result.Success());
    }
}