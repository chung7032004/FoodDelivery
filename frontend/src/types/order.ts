export type ShippingService = "Standard" | "Express";
export type PaymentMethod = 0 | 1 | 2 | 3; // 0: Cash, 1: Card, 2: Momo, 3: VnPay
export type OrderStatus = 'pending' | 'preparing' | 'delivering' | 'completed' | 'cancelled';

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

export interface CreatedOrder {
  orderId: string;
  orderCode: string;
  status: number;
  subtotal: number;
  totalAmount: number;
  shippingFee: number;
  estimatedDeliveryTime: string;
  paymentUrl?: string;
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

export interface OrderItemDetail {
  orderItemId: string;
  productName: string;
  productImage: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
  isRemoved: boolean;
  removeReason: string | null;
}

export interface OrderStatusHistory {
  status: number | string;
  changedAt: string;
  changedBy: string;
  note: string;
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
  items: OrderItemDetail[];
  statusHistories: OrderStatusHistory[];
}

