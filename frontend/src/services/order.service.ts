import type { ApiResult, PagedResponse } from "@/types/common";
import { api } from "./api";

export interface CheckoutRequest {
  addressId: string;
  cartItemIds: string[];
  shippingService: ShippingService;
  paymentMethod: PaymentMethod;
  note?: string;
}

export interface BuyNowRequest {
  productId: string;
  quantity: number;
  addressId: string;
  shippingService: ShippingService;
  paymentMethod: PaymentMethod;
  note?: string;
}

export type ShippingService = "Standard" | "Express";
export type PaymentMethod = 0 | 1 | 2;

export interface CreatedOrder {
  orderId: string;
  orderCode: string;
  status: number;
  subtotal: number;
  totalAmount: number;
  shippingFee: number;
  estimatedDeliveryTime: string;
}

export interface ShippingFeeEstimate {
  shippingFee: number;
  distance: number;
  estimatedMinutes: number;
}

export interface OrderHistoryItem {
  orderId: string;
  orderCode: string;
  createdAt: string;
  subtotal: number;
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
  subtotal: number;
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
  buyNow: async (request: BuyNowRequest): Promise<ApiResult<CreatedOrder>> => {
    const { data } = await api.post<ApiResult<CreatedOrder>>("/orders/buy-now", request);
    return data;
  },
  checkout: async (request: CheckoutRequest): Promise<ApiResult<CreatedOrder>> => {
    const { data } = await api.post<ApiResult<CreatedOrder>>("/orders/checkout", request);
    return data;
  },
  calculateShippingFee: async (
    addressId: string,
    restaurantId: string,
    subtotal: number,
    serviceType: ShippingService = "Standard",
  ): Promise<ApiResult<ShippingFeeEstimate>> => {
    const { data } = await api.get<ApiResult<ShippingFeeEstimate>>("/orders/shipping-fee", {
      params: { addressId, restaurantId, subtotal, serviceType },
    });
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