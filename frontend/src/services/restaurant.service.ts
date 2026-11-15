import { api } from './api';
import type {
  ApiResult,
  PagedResponse,
  RestaurantApplicationStatus,
  RestaurantRegistrationRequest,
  RestaurantApplication,
  MyRestaurantApplication,
  RestaurantProfile,
  PublicRestaurantProfile,
  RestaurantStatus,
} from '@/types';

export type {
  RestaurantApplicationStatus,
  RestaurantRegistrationRequest,
  RestaurantApplication,
  MyRestaurantApplication,
  RestaurantProfile,
  PublicRestaurantProfile,
  RestaurantStatus,
};

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
  getPublicProfile: async (restaurantId: string) =>
    (await api.get<ApiResult<PublicRestaurantProfile>>(`/restaurant/public/${restaurantId}`)).data,
  getStatus: async () => (await api.get<ApiResult<RestaurantStatus>>('/restaurant/status')).data,
  updateProfile: (payload: Omit<RestaurantProfile, 'id' | 'isOpen' | 'closingMessage'>) => api.put('/restaurant/profile', payload),
  open: () => api.put('/restaurant/open'),
  close: (closingMessage: string) => api.put('/restaurant/close', { closingMessage }),
};
