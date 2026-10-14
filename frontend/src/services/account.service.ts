import { ApiResult } from '@/types';
import { api } from './api';

export interface AccountResponse {
  id: string;
  fullName: string | null;
  email: string | null;
  phone: string | null;
  avatarUrl: string | null;
  roles: string[];
}

export interface UserRoleRestriction {
  restrictionId: string;
  roleName: string;
  reason: string;
  restrictedAt: string;
  appealId: string | null;
  appealReason: string | null;
  appealStatus: 'Pending' | 'Rejected' | 'Approved' | null;
  appealSubmittedAt: string | null;
  appealResponse: string | null;
}

interface ApiListResult<T> {
  data: T;
  isSuccess: boolean;
  message: string | null;
}

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
    const { data } = await api.get<ApiListResult<UserRoleRestriction[]>>('/accounts/me/role-restrictions');
    return data.data;
  },
  submitRoleAppeal: (roleName: string, reason: string) =>
    api.post('/accounts/me/role-appeals', { roleName, reason }),
};