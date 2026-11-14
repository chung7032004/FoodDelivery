import type { PaginationMeta, PagedResult } from './common';
import type { ShipperApplication } from './shipper';

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

export interface DashboardStats {
  users: number;
  orders: number;
  categories: number;
  products: number;
  pendingShippers: number;
}

export type { PaginationMeta, PagedResult, ShipperApplication };
