import type { PagedResponse } from "./common";

export interface CartItemDto {
  id: string;
  productId: string;
  quantity: number;
  categoryId: string;
  categoryName: string;
  restaurantId: string;
  productName: string;
  price: number;
  imageUrl: string | null;
  addedAt: string;
  updatedAt: string;
}

export type CartResponse = PagedResponse<CartItemDto>;