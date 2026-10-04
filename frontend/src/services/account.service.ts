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
};