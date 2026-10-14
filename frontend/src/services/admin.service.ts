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
  avatarUrl: string | null;
  isActive: boolean;
  lastLogin: string | null;
  totalOrders: number;
  totalSpent: number;
  roles: string[];
}

export interface AdminUserDetail extends AdminUser {
  avatarUrl: string;
  createdAt: string;
  updatedAt: string | null;
  recentOrders: {
    orderId: string;
    orderCode: string;
    amount: number;
    status: number;
    createdAt: string;
  }[];
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
  userId: string; // Added userId
  identityCard: string; // Added identityCard
}

export interface AdminRoleAppeal {
  appealId: string;
  userId: string;
  userName: string;
  userEmail: string;
  roleName: 'Shipper' | 'RestaurantOwner';
  restrictionReason: string;
  appealReason: string;
  submittedAt: string;
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

  getUserDetail: async (id: string) => {
    const { data } = await api.get<ApiResult<AdminUserDetail>>(`/accounts/${id}`);
    return data.data;
  },

  updateUser: (id: string, formData: FormData) =>
    api.put(`/accounts/${id}`, formData, { headers: { 'Content-Type': 'multipart/form-data' } }),

  deleteUser: (id: string) => api.delete(`/accounts/${id}`),

  blockUser: (id: string) => api.patch(`/accounts/${id}/block`),
  unblockUser: (id: string) => api.patch(`/accounts/${id}/unblock`),
  suspendUserRole: (id: string, roleName: string, reason: string) =>
    api.post(`/accounts/${id}/roles/${encodeURIComponent(roleName)}/restriction`, { reason }),
  getRoleAppeals: async () => {
    const { data } = await api.get<ApiResult<AdminRoleAppeal[]>>('/accounts/role-appeals');
    return data.data;
  },
  reviewRoleAppeal: (id: string, isApproved: boolean, response?: string) =>
    api.patch(`/accounts/role-appeals/${id}/review`, { isApproved, response }),

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