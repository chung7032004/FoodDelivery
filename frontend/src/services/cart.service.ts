import type { ApiResult } from "@/types/common";
import type { CartResponse } from "@/types/cart";
import { api } from "./api";

export const cartQueryKey = ["cart"] as const;

export const cartService = {
  getCart: async (): Promise<ApiResult<CartResponse>> => {
    try {
      const { data } = await api.get<ApiResult<CartResponse>>("/cart", {
        params: { page: 1, pageSize: 100 },
      });
      return data;
    } catch (error) {
      if (
        typeof error === "object" &&
        error !== null &&
        "statusCode" in error &&
        error.statusCode === 404
      ) {
        return {
          isSuccess: true,
          data: {
            data: [],
            meta: { page: 1, pageSize: 100, totalCount: 0, totalPages: 0 },
          },
          errorCode: null,
          message: null,
          statusCode: 200,
        };
      }
      throw error;
    }
  },

  addItem: (productId: string, quantity: number) =>
    api.post("/cart", { productId, quantity }),

  updateQuantity: (productId: string, quantity: number) =>
    api.patch(`/cart/item/${productId}`, { quantity }),

  removeItem: (productId: string) => api.delete(`/cart/item/${productId}`),
};