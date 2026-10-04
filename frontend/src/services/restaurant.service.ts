import { api } from './api';
import type { PagedResponse } from '@/types/common';

export type RestaurantApplicationStatus = 0 | 1 | 2;

export interface RestaurantRegistrationRequest {
  name: string;
  address: string;
  phone: string;
  latitude: number;
  longitude: number;
  openTime: string;
  closeTime: string;
}

export interface RestaurantApplication {
  restaurantId: string;
  ownerId: string;
  ownerName: string;
  ownerEmail: string;
  ownerPhone: string;
  name: string;
  address: string;
  phone: string;
  latitude: number;
  longitude: number;
  openTime: string;
  closeTime: string;
  isOpen: boolean;
  applicationStatus: RestaurantApplicationStatus;
  rejectionReason: string | null;
  createdAt: string;
  reviewedAt: string | null;
}

export interface MyRestaurantApplication {
  hasApplication: boolean;
  application: RestaurantApplication | null;
}

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
  getMyApplication: async () => (await api.get<ApiResult<MyRestaurantApplication>>('/restaurant/application/me')).data,
  register: async (payload: RestaurantRegistrationRequest) => (await api.post<ApiResult<null>>('/restaurant/register', payload)).data,
  getAdminRestaurants: async (page = 1, pageSize = 10, status?: RestaurantApplicationStatus) => {
    const { data } = await api.get<ApiResult<PagedResponse<RestaurantApplication>>>('/restaurant/admin', {
      params: { page, pageSize, status },
    });
    return data.data;
  },
  reviewApplication: (id: string, isApproved: boolean, rejectReason?: string) =>
    api.put(`/restaurant/admin/${id}/review`, { isApproved, rejectReason }),
  getProfile: async () => (await api.get<ApiResult<RestaurantProfile>>('/restaurant/profile')).data,
  getStatus: async () => (await api.get<ApiResult<{ isOpen: boolean; closingMessage: string | null; updatedAt: string }>>('/restaurant/status')).data,
  updateProfile: (payload: Omit<RestaurantProfile, 'id' | 'isOpen' | 'closingMessage'>) => api.put('/restaurant/profile', payload),
  open: () => api.put('/restaurant/open'),
  close: (closingMessage: string) => api.put('/restaurant/close', { closingMessage }),
};
