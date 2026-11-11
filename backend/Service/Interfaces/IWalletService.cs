using FoodDelivery.Common;
using FoodDelivery.DTOs.Wallet;
using FoodDelivery.Entities;

namespace FoodDelivery.Service.Interfaces;

public interface IWalletService
{
    Task<Result<WalletBalanceResponse>> GetBalanceAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<PagedResponse<WalletTransactionResponse>>> GetHistoryAsync(
        Guid userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task CreditAsync(
        Guid userId,
        decimal amount,
        WalletTransactionType type,
        Guid? orderId,
        string description,
        CancellationToken cancellationToken = default);
    Task DebitAsync(
        Guid userId,
        decimal amount,
        WalletTransactionType type,
        Guid? orderId,
        string description,
        CancellationToken cancellationToken = default);
}
