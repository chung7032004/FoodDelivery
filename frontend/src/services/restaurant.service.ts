import { api } from './api';

export interface RestaurantProfile {
  id: string;
  name: string;
  address: string;
  phone: string;
  isOpen: boolean;
  closingMessage: string | null;
  openTime: string;
  closeTime: string;
  latitude: number;
  longitude: number;
}

interface ApiResult<T> { data: T; isSuccess: boolean; message: string | null; }

export const restaurantService = {
  getProfile: async () => (await api.get<ApiResult<RestaurantProfile>>('/restaurant/profile')).data,
  getStatus: async () => (await api.get<ApiResult<{ isOpen: boolean; closingMessage: string | null; updatedAt: string }>>('/restaurant/status')).data,
  updateProfile: (payload: Omit<RestaurantProfile, 'id' | 'isOpen' | 'closingMessage'>) => api.put('/restaurant/profile', payload),
  open: () => api.put('/restaurant/open'),
  close: (closingMessage: string) => api.put('/restaurant/close', { closingMessage }),
};
