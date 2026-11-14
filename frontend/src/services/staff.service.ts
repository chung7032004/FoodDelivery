import { api } from './api';
import type {
  ApiResult,
  RestaurantOrderItem,
  RestaurantOrder,
  StaffStats,
  RestaurantAnalyticsPoint,
  RestaurantAnalytics,
} from '@/types';

export type {
  RestaurantOrderItem,
  RestaurantOrder,
  StaffStats,
  RestaurantAnalyticsPoint,
  RestaurantAnalytics,
};

export const staffService = {
  getStats: async () => (await api.get<ApiResult<StaffStats>>('/staff/stats')).data,
  getAnalytics: async (period: RestaurantAnalytics['period']) =>
    (await api.get<ApiResult<RestaurantAnalytics>>('/staff/analytics', { params: { period } })).data,
  getOrders: async () => (await api.get<ApiResult<RestaurantOrder[]>>('/staff/orders')).data,
  getCompletedOrders: async (days = 30) => (await api.get<ApiResult<RestaurantOrder[]>>('/staff/completed-orders', { params: { days } })).data,
  confirm: (id: string, prepMinutes?: number) => api.post(`/staff/orders/${id}/confirm`, null, { params: prepMinutes ? { prepMinutes } : undefined }),
  startPreparing: (id: string) => api.post(`/staff/orders/${id}/start-preparing`),
  markReady: (id: string) => api.post(`/staff/orders/${id}/mark-ready`),
  cancelOrder: (id: string, reason: string) => api.post(`/staff/orders/${id}/cancel`, { reason }),
  reportOutOfStock: (id: string, removedProductIds: string[], note: string) =>
    api.post(`/staff/orders/${id}/report-out-of-stock`, { removedProductIds, note }),
  reportIssue: (id: string, issueDescription: string, issueType: string) => api.post(`/staff/orders/${id}/report-issue`, { issueDescription, issueType }),
};
