import { useEffect, useMemo, useState } from 'react';
import {
  ArrowDownLeft,
  ArrowUpRight,
  ChevronLeft,
  ChevronRight,
  Wallet,
} from 'lucide-react';
import { walletService, type WalletTransactionResponse } from '@/services/wallet.service';

const PAGE_SIZE = 6;

const currencyFormatter = new Intl.NumberFormat('vi-VN', {
  style: 'currency',
  currency: 'VND',
  maximumFractionDigits: 0,
});

const dateFormatter = new Intl.DateTimeFormat('vi-VN', {
  dateStyle: 'medium',
  timeStyle: 'short',
});

function formatCurrency(value: number) {
  return currencyFormatter.format(value);
}

function formatTransactionType(type: WalletTransactionResponse['type']) {
  const labels: Record<WalletTransactionResponse['type'], string> = {
    OrderRefund: 'Hoàn tiền',
    OrderPayment: 'Thanh toán',
    TopUp: 'Nạp tiền',
    Withdraw: 'Rút tiền',
  };

  return labels[type];
}

function getTransactionDirection(type: WalletTransactionResponse['type']) {
  return type === 'OrderPayment' || type === 'Withdraw' ? 'out' : 'in';
}

function getTransactionAmount(type: WalletTransactionResponse['type'], amount: number) {
  const direction = getTransactionDirection(type);
  return direction === 'out' ? -amount : amount;
}

function getTransactionTone(type: WalletTransactionResponse['type']) {
  return getTransactionDirection(type) === 'in' ? 'text-emerald-500' : 'text-red-500';
}

export function WalletTab() {
  const [balance, setBalance] = useState(0);
  const [page, setPage] = useState(1);
  const [transactions, setTransactions] = useState<WalletTransactionResponse[]>([]);
  const [totalPages, setTotalPages] = useState(1);
  const [loading, setLoading] = useState(true);
  const [loadingMore, setLoadingMore] = useState(false);
  const [error, setError] = useState('');

  const loadWallet = async (nextPage: number) => {
    try {
      if (nextPage === 1) {
        setLoading(true);
      } else {
        setLoadingMore(true);
      }

      const [balanceResponse, historyResponse] = await Promise.all([
        walletService.getBalance(),
        walletService.getHistory(nextPage, PAGE_SIZE),
      ]);

      if (!balanceResponse.isSuccess || !historyResponse.isSuccess) {
        throw new Error(balanceResponse.message ?? historyResponse.message ?? 'Không thể tải thông tin ví.');
      }

      setBalance(balanceResponse.data.balance ?? 0);
      setTransactions(historyResponse.data.data ?? []);
      setTotalPages(historyResponse.data.meta?.totalPages ?? 1);
      setPage(historyResponse.data.meta?.page ?? nextPage);
      setError('');
    } catch (requestError) {
      const message = requestError instanceof Error ? requestError.message : 'Không thể tải thông tin ví.';
      setError(message);
    } finally {
      setLoading(false);
      setLoadingMore(false);
    }
  };

  useEffect(() => {
    void loadWallet(1);
  }, []);

  const summary = useMemo(() => {
    const topUp = transactions
      .filter((transaction) => transaction.type === 'TopUp')
      .reduce((sum, transaction) => sum + transaction.amount, 0);

    const payment = transactions
      .filter((transaction) => transaction.type === 'OrderPayment' || transaction.type === 'Withdraw')
      .reduce((sum, transaction) => sum + transaction.amount, 0);

    const refund = transactions
      .filter((transaction) => transaction.type === 'OrderRefund')
      .reduce((sum, transaction) => sum + transaction.amount, 0);

    return { topUp, payment, refund };
  }, [transactions]);

  const handlePageChange = (nextPage: number) => {
    if (nextPage < 1 || nextPage > totalPages) return;
    void loadWallet(nextPage);
  };

  return (
    <div className="space-y-6">
      <div className="flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <h2 className="font-display text-lg font-bold text-black">Ví tiền</h2>
          <p className="text-sm text-muted">Theo dõi số dư và lịch sử giao dịch của bạn.</p>
        </div>
      </div>

      {error && (
        <div className="rounded-xl border border-red-400/20 bg-red-400/10 p-4 text-sm text-red-300">
          {error}
        </div>
      )}

      {!loading && !error && (
        <>
          <div className="grid gap-4 md:grid-cols-[1.3fr_0.7fr_0.7fr_0.7fr]">
            <div className="rounded-2xl border border-brand/20 bg-gradient-to-br from-brand/20 via-orange-600/10 to-transparent p-5">
              <div className="flex items-center justify-between">
                <div className="flex items-center gap-2 text-sm font-medium text-brand">
                  <Wallet size={18} /> Số dư hiện tại
                </div>
              </div>
              <div className="mt-4 text-3xl font-bold text-black">{formatCurrency(balance)}</div>
              <p className="mt-2 text-sm text-muted">Sẵn sàng dùng cho thanh toán đơn hàng và hoàn tiền.</p>
            </div>

            <StatCard label="Nạp tiền" value={summary.topUp} tone="success" />
            <StatCard label="Thanh toán" value={summary.payment} tone="danger" />
            <StatCard label="Hoàn tiền" value={summary.refund} tone="info" />
          </div>

          <div className="rounded-2xl border border-border bg-white/[0.02] p-4 sm:p-5">
            <div className="mb-4 flex items-center justify-between gap-2">
              <h3 className="text-base font-semibold text-black">Lịch sử giao dịch</h3>
              <span className="text-xs text-muted">Trang {page}/{totalPages}</span>
            </div>

            {transactions.length === 0 ? (
              <div className="rounded-xl border border-dashed border-border bg-surface/50 px-4 py-8 text-center text-sm text-muted">
                Chưa có giao dịch nào.
              </div>
            ) : (
              <div className="space-y-3">
                {transactions.map((transaction) => {
                  const signedAmount = getTransactionAmount(transaction.type, transaction.amount);
                  const isIncome = getTransactionDirection(transaction.type) === 'in';

                  return (
                    <div
                      key={transaction.id}
                      className="flex flex-col gap-3 rounded-xl border border-border bg-slate-900/20 p-3 sm:flex-row sm:items-center sm:justify-between"
                    >
                      <div className="flex items-start gap-3">
                        <div className={`mt-0.5 rounded-lg p-2 ${isIncome ? 'bg-emerald-500/10 text-emerald-500' : 'bg-red-500/10 text-red-500'}`}>
                          {isIncome ? <ArrowDownLeft size={16} /> : <ArrowUpRight size={16} />}
                        </div>
                        <div>
                          <div className="flex flex-wrap items-center gap-2">
                            <span className="font-medium text-black">{formatTransactionType(transaction.type)}</span>
                            <span className="rounded-full bg-white/5 px-2 py-0.5 text-[11px] font-medium text-muted">
                              {transaction.orderId ? 'Đơn hàng' : 'Ví'}
                            </span>
                          </div>
                          <p className="mt-1 text-sm text-muted">{transaction.description || 'Không có mô tả'}</p>
                          <p className="mt-1 text-xs text-muted">{dateFormatter.format(new Date(transaction.createdAt))}</p>
                        </div>
                      </div>

                      <div className="text-left sm:text-right">
                        <div className={`text-base font-semibold ${getTransactionTone(transaction.type)}`}>
                          {signedAmount >= 0 ? '+' : '-'}{formatCurrency(Math.abs(signedAmount))}
                        </div>
                        <div className="text-xs text-muted">Số dư sau: {formatCurrency(transaction.balanceAfter)}</div>
                      </div>
                    </div>
                  );
                })}
              </div>
            )}

            {totalPages > 1 && (
              <div className="mt-5 flex items-center justify-center gap-3">
                <button
                  type="button"
                  onClick={() => handlePageChange(page - 1)}
                  disabled={page <= 1 || loadingMore}
                  className="inline-flex items-center gap-1 rounded-lg border border-border px-3 py-2 text-sm font-medium text-muted transition hover:border-brand hover:text-black disabled:cursor-not-allowed disabled:opacity-40"
                >
                  <ChevronLeft size={15} /> Trước
                </button>
                <span className="text-sm text-muted">{page} / {totalPages}</span>
                <button
                  type="button"
                  onClick={() => handlePageChange(page + 1)}
                  disabled={page >= totalPages || loadingMore}
                  className="inline-flex items-center gap-1 rounded-lg border border-border px-3 py-2 text-sm font-medium text-muted transition hover:border-brand hover:text-black disabled:cursor-not-allowed disabled:opacity-40"
                >
                  Sau <ChevronRight size={15} />
                </button>
              </div>
            )}
          </div>
        </>
      )}

      {loading && (
        <div className="flex min-h-48 items-center justify-center rounded-2xl border border-border bg-white/[0.02]">
          <div className="h-8 w-8 animate-spin rounded-full border-2 border-brand border-t-transparent" />
        </div>
      )}
    </div>
  );
}

function StatCard({ label, value, tone }: { label: string; value: number; tone: 'success' | 'danger' | 'info' }) {
  const toneClasses = {
    success: 'border-emerald-500/20 bg-emerald-500/10 text-emerald-500',
    danger: 'border-red-500/20 bg-red-500/10 text-red-500',
    info: 'border-sky-500/20 bg-sky-500/10 text-sky-500',
  };

  return (
    <div className={`rounded-2xl border p-4 ${toneClasses[tone]}`}>
      <p className="text-sm font-medium">{label}</p>
      <p className="mt-3 text-xl font-bold text-black">{formatCurrency(value)}</p>
    </div>
  );
}
