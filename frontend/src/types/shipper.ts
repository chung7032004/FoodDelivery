export interface ShipperProfile {
  shipperId: string;
  userId: string;
  status: string | number;
  isActive: boolean;
  isAvailable: boolean;
  totalDeliveredOrders: number;
  updatedAt: string;
}

export interface ShipperApplicationStatus {
  hasApplication: boolean;
  shipperId: string | null;
  status: string | number | null;
  identityCard: string | null;
  vehicleType: string | null;
  vehiclePlate: string | null;
  rejectReason: string | null;
  appliedAt: string | null;
  reviewedAt: string | null;
}

export interface ApplyShipperRequest {
  identityCard: string;
  vehicleType: string;
  vehiclePlate: string;
}

export interface ShipperStats {
  totalAssignedOrders: number;
  totalCompletedOrders: number;
  totalFailedOrders: number;
  totalEarnings: number;
  averageRating: number;
}

export interface ShipperOrder {
  id: string;
  orderCode: string;
  customerName: string;
  totalAmount: number;
  shippingFee: number;
  shippingAddress?: string | null;
  status: string;
  dispatchStatus?: string;
  prepMinutes?: number | null;
  createdAt: string;
}

export interface ShipperOrderItem {
  productName: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
}

export interface ShipperOrderDetail {
  orderId: string;
  orderCode: string;
  customerName: string;
  customerEmail: string;
  shippingAddress: string;
  receiverName: string;
  receiverPhone: string;
  shippingFee: number;
  estimatedDeliveryTime: string | null;
  actualDeliveryTime: string | null;
  paymentMethod: string;
  paymentStatus: string;
  currentStatus: string;
  cancelReason: string | null;
  totalAmount: number;
  shipperId: string | null;
  items: ShipperOrderItem[];
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
  userId: string;
  identityCard: string;
}

