import { decodeAccessToken, getRolesFromPayload } from '@/utils/jwt';
import { api, tokenStore } from './api';
import type {
  LoginRequest,
  RegisterRequest,
  ChangePasswordRequest,
  LoginResponse,
  AuthUser,
} from '@/types';

export type {
  LoginRequest,
  RegisterRequest,
  ChangePasswordRequest,
  LoginResponse,
  AuthUser,
};

function buildUserFromToken (token: string): AuthUser | null {
  const payload = decodeAccessToken(token);
  if (!payload) return null;
  return {
    id: payload.sub,
    name: payload.name,
    email: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] || undefined,
    roles: getRolesFromPayload(payload)
  };
}

export const authService = {
  login: async (data: LoginRequest) => {
    const res = await api.post<{ accessToken: string }>('/auth/login', data);
    console.log('Login response:', res.data);
    tokenStore.set(res.data.accessToken);
    const user = buildUserFromToken(res.data.accessToken);
    return { accessToken: res.data.accessToken, user };
  },

  register: (data: RegisterRequest) =>
    api.post('/auth/register', data).then((res) => res.data),

  logout: async () => {
    try {
      await api.post('/auth/logout');
    } catch (error) {
      console.error('Logout failed:', error);
    } finally {
      tokenStore.clear();
    }
  },

  refresh: async () => {
    const res = await api.post<LoginResponse>('/auth/refresh');
    tokenStore.set(res.data.accessToken);
    return res.data;
  },        

  changePassword: (data: ChangePasswordRequest) =>
    api.post('/auth/change-password', data).then((res) => res.data),

  sendOtp: (email: string) =>
    api.post('/auth/send-otp', { email }).then((res) => res.data),

  resetPassword: (email: string, otp: string, newPassword: string) =>
    api.post('/auth/reset-password', { email, otp, newPassword }).then((res) => res.data),
  
  getCurrentUser: (): AuthUser | null => {
    const token = tokenStore.get();
    if (!token) return null;
    return buildUserFromToken(token);
  },
};
