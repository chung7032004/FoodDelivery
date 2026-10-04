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
    create: (payload: FormData) => api.post('/products', payload, { headers: { 'Content-Type': 'multipart/form-data' } }),
    update: (id: string, payload: FormData) => api.put(`/products/${id}`, payload, { headers: { 'Content-Type': 'multipart/form-data' } }),
    toggle: (id: string) => api.patch(`/products/${id}/toggle`),
    remove: (id: string) => api.delete(`/products/${id}`),
}