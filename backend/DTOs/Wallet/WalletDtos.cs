namespace FoodDelivery.DTOs.Wallet;

/// <summary>Current balance of the authenticated user's wallet.</summary>
public sealed record WalletBalanceResponse(decimal Balance);

/// <summary>One wallet ledger transaction.</summary>
public sealed record WalletTransactionResponse(
    Guid Id,
    string Type,
    decimal Amount,
    decimal BalanceAfter,
    Guid? OrderId,
    string Description,
    DateTimeOffset CreatedAt);
