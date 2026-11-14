import { api } from "./api";
import type { ApiResult, PagedResponse, CategoryQueryParams, CategoryResponse } from "@/types";

export type { CategoryResponse };

export const categoryService = {
  getAll: async (params: CategoryQueryParams): Promise<ApiResult<PagedResponse<CategoryResponse>>> => {
    const { data } = await api.get<ApiResult<PagedResponse<CategoryResponse>>>('/categories', {
      params: { search: params.searchQuery, page: params.page, pageSize: params.pageSize },
    });
    return data;
  },
  create: (payload: FormData) => api.post('/categories', payload, { headers: { 'Content-Type': 'multipart/form-data' } }),
  update: (id: string, payload: FormData) => api.put(`/categories/${id}`, payload, { headers: { 'Content-Type': 'multipart/form-data' } }),
  remove: (id: string) => api.delete(`/categories/${id}`),
};
