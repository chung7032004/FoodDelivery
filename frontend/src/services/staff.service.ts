import { api } from './api';

export interface RestaurantOrderItem { productId: string; productName: string; quantity: number; price: number; }
export interface RestaurantOrder { id: string; orderNumber: string; customerName: string; customerPhone: string; totalAmount: number; status: number; createdAt: string; items: RestaurantOrderItem[]; }
export interface StaffStats {
  totalOrdersToday: number;
  pendingCount: number;
  waitingCustomerDecisionCount: number;
  confirmedCount: number;
  preparingCount: number;
  readyCount: number;
  shippingCount: number;
}
export interface RestaurantAnalyticsPoint {
  periodStart: string;
  revenue: number;
  grossProfit: number | null;
  completedOrders: number;
  ordersMissingCost: number;
}
export interface RestaurantAnalytics {
  period: 'daily' | 'monthly' | 'yearly';
  revenue: number;
  grossProfit: number | null;
  completedOrders: number;
  ordersMissingCost: number;
  points: RestaurantAnalyticsPoint[];
}
interface ApiResult<T> { data: T; isSuccess: boolean; message: string | null; }

export const staffService = {
  getStats: async () => (await api.get<ApiResult<StaffStats>>('/staff/stats')).data,
  getAnalytics: async (period: RestaurantAnalytics['period']) =>
    (await api.get<ApiResult<RestaurantAnalytics>>('/staff/analytics', { params: { period } })).data,
  getOrders: async () => (await api.get<ApiResult<RestaurantOrder[]>>('/staff/orders')).data,
  getCompletedOrders: async (days = 30) => (await api.get<ApiResult<RestaurantOrder[]>>('/staff/completed-orders', { params: { days } })).data,
  confirm: (id: string) => api.post(`/staff/orders/${id}/confirm`),
  startPreparing: (id: string) => api.post(`/staff/orders/${id}/start-preparing`),
  markReady: (id: string) => api.post(`/staff/orders/${id}/mark-ready`),
  reportOutOfStock: (id: string, removedProductIds: string[], note: string) =>
    api.post(`/staff/orders/${id}/report-out-of-stock`, { removedProductIds, note }),
  reportIssue: (id: string, issueDescription: string, issueType: string) => api.post(`/staff/orders/${id}/report-issue`, { issueDescription, issueType }),
};
