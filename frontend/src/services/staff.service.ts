import { api } from './api';

export interface RestaurantOrderItem { productName: string; quantity: number; price: number; }
export interface RestaurantOrder { id: string; orderNumber: string; customerName: string; customerPhone: string; totalAmount: number; status: number; createdAt: string; items: RestaurantOrderItem[]; }
export interface StaffStats { totalOrdersToday: number; preparingCount: number; readyCount: number; issueCount: number; }
interface ApiResult<T> { data: T; isSuccess: boolean; message: string | null; }

export const staffService = {
  getStats: async () => (await api.get<ApiResult<StaffStats>>('/staff/stats')).data,
  getOrders: async () => (await api.get<ApiResult<RestaurantOrder[]>>('/staff/orders')).data,
  getCompletedOrders: async (days = 30) => (await api.get<ApiResult<RestaurantOrder[]>>('/staff/completed-orders', { params: { days } })).data,
  confirm: (id: string) => api.post(`/staff/orders/${id}/confirm`),
  startPreparing: (id: string) => api.post(`/staff/orders/${id}/start-preparing`),
  markReady: (id: string) => api.post(`/staff/orders/${id}/mark-ready`),
  reportIssue: (id: string, issueDescription: string, issueType: string) => api.post(`/staff/orders/${id}/report-issue`, { issueDescription, issueType }),
};
