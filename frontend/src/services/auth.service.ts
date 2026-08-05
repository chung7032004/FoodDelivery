import { api, tokenStore } from './api';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  fullName: string;
  phone: string;
}

export interface ChangePasswordRequest {
  oldPassword: string;
  newPassword: string;
  confirmNewPassword: string;
}

export interface LoginResponse {
  accessToken: string;
}


export const authService = {
  login: async (data: LoginRequest) => {
    const res = await api.post<{ accessToken: string }>('/auth/login', data);
    tokenStore.set(res.data.accessToken);
    return res.data;
  },

  register: (data: RegisterRequest) =>
    api.post('/auth/register', data).then((res) => res.data),

  logout: async () => {
    await api.post('/auth/logout');
    tokenStore.clear();
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
};