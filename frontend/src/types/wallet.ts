import type { PaginationMeta } from './common';

export interface WalletBalanceResponse {
  balance: number;
}

export interface WalletTransactionResponse {
  id: string;
  type: 'OrderRefund' | 'OrderPayment' | 'TopUp' | 'Withdraw';
  amount: number;
  balanceAfter: number;
  orderId: string | null;
  description: string;
  createdAt: string;
}

export interface WalletHistoryPage {
  data: WalletTransactionResponse[];
  meta: PaginationMeta;
}

