using FoodDelivery.Entities;
namespace FoodDelivery.Service.Interfaces;
public interface IVnPayService
{
    string CreatePaymentUrl(Order order, HttpContext httpContext);
    bool ValidateCallback(IQueryCollection query);
}