import type { ApiResult, PagedResponse } from "@/types/common";
import { api } from "./api";

export interface CheckoutRequest {
  addressId: string;
  cartItemIds: string[];
  note?: string;
}

export interface CreatedOrder {
  orderId: string;
  orderCode: string;
  status: number;
  totalAmount: number;
  shippingFee: number;
  estimatedDeliveryTime: string;
}

export interface OrderHistoryItem {
  orderId: string;
  orderCode: string;
  createdAt: string;
  totalAmount: number;
  shippingFee: number;
  estimatedDeliveryTime: string | null;
  currentStatus: number | string;
}

export interface OrderDetail {
  orderId: string;
  orderCode: string;
  createdAt: string;
  shippingAddress: string;
  receiverName: string;
  receiverPhone: string;
  shippingFee: number;
  estimatedDeliveryTime: string | null;
  actualDeliveryTime: string | null;
  paymentMethod: number | string;
  paymentStatus: number | string;
  currentStatus: number | string;
  cancelReason: string | null;
  totalAmount: number;
  items: {
    orderItemId: string;
    productName: string;
    productImage: string;
    quantity: number;
    unitPrice: number;
    totalPrice: number;
    isRemoved: boolean;
    removeReason: string | null;
  }[];
  statusHistories: {
    status: number | string;
    changedAt: string;
    changedBy: string;
    note: string;
  }[];
}

export const orderService = {
  checkout: async (request: CheckoutRequest): Promise<ApiResult<CreatedOrder>> => {
    const { data } = await api.post<ApiResult<CreatedOrder>>("/orders/checkout", request);
    return data;
  },
  getMyHistory: async (page = 1, pageSize = 5): Promise<ApiResult<PagedResponse<OrderHistoryItem>>> => {
    const { data } = await api.get<ApiResult<PagedResponse<OrderHistoryItem>>>("/orders/history", {
      params: { page, pageSize },
    });
    return data;
  },
  getDetail: async (orderId: string): Promise<ApiResult<OrderDetail>> => {
    const { data } = await api.get<ApiResult<OrderDetail>>(`/orders/${orderId}`);
    return data;
  },
};