import type { AccountResponse, UserRoleRestriction, ApiResult } from '@/types';
import { api } from './api';

export type { AccountResponse, UserRoleRestriction };

export const accountService = {
  getMe: async (): Promise<ApiResult<AccountResponse>> => {
    const { data } = await api.get<ApiResult<AccountResponse>>('/accounts/me');
    return data;
  },
  updateMe: async (formData: FormData): Promise<ApiResult<null>> => {
    const { data } = await api.put<ApiResult<null>>('/accounts/me', formData, {
      headers: { 'Content-Type': 'multipart/form-data' },
    });
    return data;
  },
  getMyRoleRestrictions: async () => {
    const { data } = await api.get<ApiResult<UserRoleRestriction[]>>('/accounts/me/role-restrictions');
    return data.data;
  },
  submitRoleAppeal: (roleName: string, reason: string) =>
    api.post('/accounts/me/role-appeals', { roleName, reason }),
};
