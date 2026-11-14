export interface RestaurantOrderItem {
  productId: string;
  productName: string;
  quantity: number;
  price: number;
}

export interface RestaurantOrder {
  id: string;
  orderNumber: string;
  customerName: string;
  customerPhone: string;
  shipperName: string | null;
  shipperPhone: string | null;
  shipperVehicleType: string | null;
  shipperVehiclePlate: string | null;
  totalAmount: number;
  status: number;
  prepMinutes: number | null;
  createdAt: string;
  items: RestaurantOrderItem[];
}

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

