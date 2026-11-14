using FoodDelivery.Service.Interfaces.IPayment;

namespace FoodDelivery.Service.Implements.Payment;
public class PaymentGatewayResolver : IPaymentGatewayResolver
{
    private readonly Dictionary<PaymentMethod, IPaymentGateway> _gateways;
     // .NET DI tự động inject TẤT CẢ các class kế thừa IPaymentGateway vào đây!
    public PaymentGatewayResolver(IEnumerable<IPaymentGateway> gateways)
    {
         _gateways = gateways.ToDictionary(g => g.Method);
    }
    public IPaymentGateway GetGateway(PaymentMethod method)
    {
        if(_gateways.TryGetValue(method, out var gateway))
        {
            return gateway;
        }
        throw new NotSupportedException($"Payment method {method} is not supported.");
    }

    public bool IsSupported(PaymentMethod method)
    {
        return _gateways.ContainsKey(method);
    }

    public IReadOnlyCollection<PaymentMethod> GetSupportedMethods()
    {
        return _gateways.Keys.ToList();
    }
}