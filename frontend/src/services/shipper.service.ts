import { api } from './api';

export interface ShipperProfile { shipperId: string; userId: string; status: string | number; isActive: boolean; isAvailable: boolean; totalDeliveredOrders: number; updatedAt: string; }
export interface ShipperStats { totalAssignedOrders: number; totalCompletedOrders: number; totalFailedOrders: number; totalEarnings: number; averageRating: number; }
export interface ShipperOrder { id: string; orderCode: string; customerName: string; totalAmount: number; shippingAddress?: string | null; status: string; createdAt: string; }
export interface OrderDetail { orderId: string; orderCode: string; customerName: string; customerEmail: string; shippingAddress: string; receiverName: string; receiverPhone: string; shippingFee: number; estimatedDeliveryTime: string | null; actualDeliveryTime: string | null; paymentMethod: string; paymentStatus: string; currentStatus: string; cancelReason: string | null; totalAmount: number; shipperId: string | null; items: { productName: string; quantity: number; unitPrice: number; totalPrice: number }[]; }

export const shipperService = {
  getProfile: async () => (await api.get<ShipperProfile>('/shipper/profile')).data,
  getStats: async () => (await api.get<{ data: ShipperStats }>('/shipper/dashboard')).data.data,
  getAvailableOrders: async () => (await api.get<ShipperOrder[]>('/shipper/orders/available')).data,
  getAssignedOrders: async () => (await api.get<ShipperOrder[]>('/shipper/orders/assigned')).data,
  getCompletedOrders: async () => (await api.get<ShipperOrder[]>('/shipper/orders/completed')).data,
  getOrderDetail: async (id: string) => (await api.get<OrderDetail>(`/shipper/orders/${id}`)).data,
  updateAvailability: (isAvailable: boolean) => api.put('/shipper/profile', { isAvailable }),
  acceptOrder: (id: string) => api.post(`/shipper/orders/${id}/accept`),
  confirmPickup: (id: string) => api.post(`/shipper/orders/${id}/pickup`),
  complete: (id: string) => api.post(`/shipper/orders/${id}/complete`),
  fail: (id: string, reason: string) => api.post(`/shipper/orders/${id}/fail`, { orderId: id, reason }),
};
