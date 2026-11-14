import type { ApiResult, CustomerAddress, AddressRequest } from "@/types";
import { api } from "./api";

export type { CustomerAddress, AddressRequest };

export const addressService = {
  getMine: async (): Promise<ApiResult<CustomerAddress[]>> => {
    const { data } = await api.get<ApiResult<CustomerAddress[]>>("/address");
    return data;
  },
  getById: async (id: string): Promise<ApiResult<CustomerAddress>> => {
    const { data } = await api.get<ApiResult<CustomerAddress>>(`/address/${id}`);
    return data;
  },
  create: (request: AddressRequest) => api.post("/address", request),
  update: (id: string, request: AddressRequest) => api.put(`/address/${id}`, request),
  remove: (id: string) => api.delete(`/address/${id}`),
  setDefault: (id: string) => api.patch(`/address/${id}/default`),
};
