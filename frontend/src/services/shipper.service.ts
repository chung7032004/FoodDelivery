import { api } from './api';
import type {
  ApiResult,
  ShipperProfile,
  ShipperApplicationStatus,
  ApplyShipperRequest,
  ShipperStats,
  ShipperOrder,
  ShipperOrderDetail,
} from '@/types';

export type {
  ShipperProfile,
  ShipperApplicationStatus,
  ApplyShipperRequest,
  ShipperStats,
  ShipperOrder,
  ShipperOrderDetail,
  ShipperOrderDetail as OrderDetail,
};

export const shipperService = {
  getMyApplication: async () => (await api.get<ApiResult<ShipperApplicationStatus>>('/shipper/application/me')).data,
  apply: async (request: ApplyShipperRequest) => (await api.post<ApiResult<string>>('/shipper/apply', request)).data,
  getProfile: async () => (await api.get<ShipperProfile>('/shipper/profile')).data,
  getStats: async () => (await api.get<{ data: ShipperStats }>('/shipper/dashboard')).data.data,
  getAvailableOrders: async () => (await api.get<ShipperOrder[]>('/shipper/orders/available')).data,
  getAssignedOrders: async () => (await api.get<ShipperOrder[]>('/shipper/orders/assigned')).data,
  getCompletedOrders: async () => (await api.get<ShipperOrder[]>('/shipper/orders/completed')).data,
  getOrderDetail: async (id: string) => (await api.get<ShipperOrderDetail>(`/shipper/orders/${id}`)).data,
  updateAvailability: (isAvailable: boolean) => api.put('/shipper/profile', { isAvailable }),
  acceptOrder: (id: string) => api.post(`/shipper/orders/${id}/accept`),
  confirmPickup: (id: string) => api.post(`/shipper/orders/${id}/pickup`),
  complete: (id: string) => api.post(`/shipper/orders/${id}/complete`),
  fail: (id: string, reason: string) => api.post(`/shipper/orders/${id}/fail`, { orderId: id, reason }),
  releaseOrder: (id: string, reason: string) => api.post(`/shipper/orders/${id}/release`, { orderId: id, reason }),
};
