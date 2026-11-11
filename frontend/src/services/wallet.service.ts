import { type ApiResult } from '@/types';
import { api } from './api';

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

export interface PaginationMeta {
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

export interface WalletHistoryPage {
  data: WalletTransactionResponse[];
  meta: PaginationMeta;
}

export const walletService = {
  getBalance: async (): Promise<ApiResult<WalletBalanceResponse>> => {
    const { data } = await api.get<ApiResult<WalletBalanceResponse>>('/wallet');
    return data;        
  },
  getHistory: async (page = 1, pageSize = 10): Promise<ApiResult<WalletHistoryPage>> => {
    const { data } = await api.get<ApiResult<WalletHistoryPage>>('/wallet/transactions', {
      params: { page, pageSize },
    });
    return data;
  },
};
