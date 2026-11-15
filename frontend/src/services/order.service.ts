import { api } from "./api";
import type {
  ApiResult,
  PagedResponse,
  CheckoutRequest,
  BuyNowRequest,
  ShippingService,
  PaymentMethod,
  CreatedOrder,
  ShippingFeeEstimate,
  OrderHistoryItem,
  OrderDetail,
  OrderItemDetail,
  OrderStatusHistory,
} from "@/types";

export type {
  CheckoutRequest,
  BuyNowRequest,
  ShippingService,
  PaymentMethod,
  CreatedOrder,
  ShippingFeeEstimate,
  OrderHistoryItem,
  OrderDetail,
  OrderItemDetail,
  OrderStatusHistory,
};

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
  respondToProposal: async (orderId: string, isAccepted: boolean): Promise<ApiResult<null>> => {
    const { data } = await api.post<ApiResult<null>>(`/orders/${orderId}/respond-proposal`, { isAccepted });
    return data;
  },
  cancelOrder: async (orderId: string, reason: string): Promise<ApiResult<null>> => {
    const { data } = await api.post<ApiResult<null>>(`/orders/${orderId}/cancel`, { reason });
    return data;
  },
};
