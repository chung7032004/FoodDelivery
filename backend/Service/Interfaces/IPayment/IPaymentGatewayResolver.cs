using FoodDelivery.Constants;

namespace FoodDelivery.Service.Interfaces.IPayment;

public interface IPaymentGatewayResolver
{
    IPaymentGateway GetGateway(PaymentMethod method);
    bool IsSupported(PaymentMethod method);
    IReadOnlyCollection<PaymentMethod> GetSupportedMethods();
}