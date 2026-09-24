import { api } from './api';

export interface PaginationMeta {
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

export interface PagedResult<T> {
  data: T[];
  meta: PaginationMeta;
}

export interface AdminUser {
  id: string;
  fullName: string;
  email: string;
  phone: string;
  isActive: boolean;
  lastLogin: string | null;
  totalOrders: number;
  totalSpent: number;
  roles: string[];
}

export interface AdminOrder {
  id: string;
  orderCode: string;
  customerName: string;
  totalAmount: number;
  status: string | number;
  createdAt: string;
  paymentMethod: string | number;
}

export interface AdminReview {
  id: string;
  customerName: string;
  productName: string;
  rating: number;
  comment: string | null;
  isHidden: boolean;
  createdAt: string;
}

export interface ShipperApplication {
  shipperId: string;
  fullName: string;
  email: string;
  phone: string;
  vehicleType: string;
  vehiclePlate: string;
  status: string | number;
  appliedAt: string;
}

interface ApiResult<T> {
  data: T;
  isSuccess: boolean;
  message: string | null;
}

export const adminService = {
  getUsers: async (params: { searchQuery?: string; isActive?: boolean; page?: number; pageSize?: number }) => {
    const { data } = await api.get<ApiResult<PagedResult<AdminUser>>>('/accounts', { params });
    return data.data;
  },

  blockUser: (id: string) => api.patch(`/accounts/${id}/block`),
  unblockUser: (id: string) => api.patch(`/accounts/${id}/unblock`),

  getOrders: async (params: { searchCode?: string; status?: string; page?: number; pageSize?: number }) => {
    const { data } = await api.get<ApiResult<PagedResult<AdminOrder>>>('/admin/orders', { params });
    return data.data;
  },

  getReviews: async (params: { rating?: number; isHidden?: boolean; page?: number; pageSize?: number }) => {
    const { data } = await api.get<ApiResult<PagedResult<AdminReview>>>('/admin/reviews', { params });
    return data.data;
  },

  hideReview: (id: string) => api.patch(`/admin/reviews/${id}/hide`),
  deleteReview: (id: string) => api.delete(`/admin/reviews/${id}`),

  getPendingShippers: async (page = 1, pageSize = 10) => {
    const { data } = await api.get<ApiResult<PagedResult<ShipperApplication>>>('/shipper/applications/pending', {
      params: { page, pageSize },
    });
    return data.data;
  },

  reviewShipper: (id: string, isApproved: boolean, rejectReason?: string) =>
    api.put(`/shipper/applications/${id}/review`, { isApproved, rejectReason }),
};