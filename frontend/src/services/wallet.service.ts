import { api } from './api';
import type {
  ApiResult,
  WalletBalanceResponse,
  WalletTransactionResponse,
  PaginationMeta,
  WalletHistoryPage,
} from '@/types';

export type {
  WalletBalanceResponse,
  WalletTransactionResponse,
  PaginationMeta,
  WalletHistoryPage,
};

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
