using FoodDelivery.Common;
using FoodDelivery.DTOs.Wallet;
using FoodDelivery.Entities;
using FoodDelivery.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Service.Implements;

public sealed class WalletService : IWalletService
{
    private readonly FoodContext _context;

    public WalletService(FoodContext context)
    {
        _context = context;
    }

    public async Task<Result<WalletBalanceResponse>> GetBalanceAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var balance = await _context.Wallets
            .AsNoTracking()
            .Where(wallet => wallet.UserId == userId)
            .Select(wallet => (decimal?)wallet.Balance)
            .SingleOrDefaultAsync(cancellationToken) ?? 0m;

        return Result<WalletBalanceResponse>.Success(new WalletBalanceResponse(balance));
    }

    public async Task<Result<PagedResponse<WalletTransactionResponse>>> GetHistoryAsync(
        Guid userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var walletId = await _context.Wallets
            .AsNoTracking()
            .Where(wallet => wallet.UserId == userId)
            .Select(wallet => (Guid?)wallet.Id)
            .SingleOrDefaultAsync(cancellationToken);

        var query = walletId.HasValue
            ? _context.WalletTransactions.AsNoTracking().Where(transaction => transaction.WalletId == walletId.Value)
            : _context.WalletTransactions.AsNoTracking().Where(_ => false);
        var totalCount = await query.CountAsync(cancellationToken);
        var records = await query
            .OrderByDescending(transaction => transaction.CreatedAt)
            .ThenByDescending(transaction => transaction.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(transaction => new
            {
                transaction.Id,
                transaction.Type,
                transaction.Amount,
                transaction.BalanceAfter,
                transaction.OrderId,
                transaction.Description,
                transaction.CreatedAt
            })
            .ToListAsync(cancellationToken);
        var transactions = records.Select(transaction => new WalletTransactionResponse(
            transaction.Id,
            transaction.Type.ToString(),
            transaction.Amount,
            transaction.BalanceAfter,
            transaction.OrderId,
            transaction.Description,
            new DateTimeOffset(DateTime.SpecifyKind(transaction.CreatedAt, DateTimeKind.Utc))))
            .ToList();

        return Result<PagedResponse<WalletTransactionResponse>>.Success(new PagedResponse<WalletTransactionResponse>(
            Data: transactions,
            Meta: new PaginationMeta(
                Page: page,
                PageSize: pageSize,
                TotalCount: totalCount,
                TotalPages: (int)Math.Ceiling((double)totalCount / pageSize))));
    }

    public Task CreditAsync(
        Guid userId,
        decimal amount,
        WalletTransactionType type,
        Guid? orderId,
        string description,
        CancellationToken cancellationToken = default) =>
        ChangeBalanceAsync(userId, amount, type, orderId, description, credit: true, cancellationToken);

    public Task DebitAsync(
        Guid userId,
        decimal amount,
        WalletTransactionType type,
        Guid? orderId,
        string description,
        CancellationToken cancellationToken = default) =>
        ChangeBalanceAsync(userId, amount, type, orderId, description, credit: false, cancellationToken);

    private async Task ChangeBalanceAsync(
        Guid userId,
        decimal amount,
        WalletTransactionType type,
        Guid? orderId,
        string description,
        bool credit,
        CancellationToken cancellationToken)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Transaction amount must be greater than zero.");
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("A transaction description is required.", nameof(description));
        if (type == WalletTransactionType.OrderRefund && orderId is null)
            throw new ArgumentException("Order refunds must reference an order.", nameof(orderId));
        if (credit != (type is WalletTransactionType.OrderRefund or WalletTransactionType.TopUp))
            throw new ArgumentException("Transaction type does not match the wallet balance direction.", nameof(type));

        await using var transaction = _context.Database.CurrentTransaction is null
            ? await _context.Database.BeginTransactionAsync(cancellationToken)
            : null;

        if (type == WalletTransactionType.OrderRefund
            && await _context.WalletTransactions.AnyAsync(
                entry => entry.OrderId == orderId && entry.Type == type,
                cancellationToken))
        {
            if (transaction is not null)
                await transaction.CommitAsync(cancellationToken);
            return;
        }

        var wallet = await _context.Wallets
            .SingleOrDefaultAsync(entry => entry.UserId == userId, cancellationToken);

        if (wallet is null)
        {
            wallet = new Wallet { UserId = userId, Balance = 0m, RowVersion = 0 };
            _context.Wallets.Add(wallet);
        }

        if (!credit && wallet.Balance < amount)
            throw new AppException("INSUFFICIENT_WALLET_BALANCE", "Số dư ví không đủ.", 409);

        wallet.Balance = credit ? wallet.Balance + amount : wallet.Balance - amount;
        wallet.RowVersion++;
        _context.WalletTransactions.Add(new WalletTransaction
        {
            Wallet = wallet,
            Type = type,
            Amount = amount,
            BalanceAfter = wallet.Balance,
            OrderId = orderId,
            Description = description.Trim(),
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync(cancellationToken);
        if (transaction is not null)
            await transaction.CommitAsync(cancellationToken);
    }
}
