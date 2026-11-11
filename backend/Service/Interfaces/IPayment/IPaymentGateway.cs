using FoodDelivery.Common;
using FoodDelivery.Entities;
namespace FoodDelivery.Service.Interfaces.IPayment;
public interface IPaymentGateway{
    PaymentMethod Method {get;}
    Task<string?> CreatePaymentUrlAsync(Order order, HttpContext httpContext);
    Task<Result> ProcessCallbackAsync(IQueryCollection query, CancellationToken ct = default);

}