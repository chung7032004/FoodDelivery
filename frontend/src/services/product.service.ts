import { ApiResult } from "@/types";
import { PagedResponse } from "@/types/common";
import { ProductDetailResponseDto, ProductQueryParams, ProductResponseDto } from "@/types/product";
import { api } from "./api";

export const productService = {
    getAll : async (params : ProductQueryParams) : Promise<ApiResult<PagedResponse<ProductResponseDto>>> =>{
        const {data} = await api.get<ApiResult<PagedResponse<ProductResponseDto>>>("/products", { params: { categoryId: params.categoryId, q: params.searchQuery, page: params.page, pageSize: params.pageSize } });
        return data;
    },
    getDetailById : async (id : string ) : Promise<ApiResult<ProductDetailResponseDto>> =>{
        const {data} = await api.get<ApiResult<ProductDetailResponseDto>>('/products/' + id);
        return data ;
    },
    create: (payload: { restaurantId: string; categoryId: string; name: string; description: string; price: number; isAvailable: boolean; isFeatured: boolean; displayOrder: number }) => {
        const form = new FormData();
        Object.entries(payload).forEach(([key, value]) => form.append(key, String(value)));
        return api.post('/products', form, { headers: { 'Content-Type': 'multipart/form-data' } });
    },
    update: (id: string, payload: { name: string; price: number; isAvailable: boolean; isFeatured: boolean; displayOrder: number }) => api.put(`/products/${id}`, payload),
    toggle: (id: string) => api.patch(`/products/${id}/toggle`),
    remove: (id: string) => api.delete(`/products/${id}`),
}