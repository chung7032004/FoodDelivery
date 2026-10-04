import type { ApiResult } from "@/types/common";
import { api } from "./api";

export interface CustomerAddress {
  id: string;
  fullAddress: string | null;
  receiverName: string | null;
  phoneNumber: string | null;
  label: string | null;
  isDefault: boolean;
  latitude: number;
  longitude: number;
}

export interface AddressRequest {
  fullAddress: string;
  label: string;
  receiverName: string;
  phoneNumber: string;
  latitude: number;
  longitude: number;
  isDefault: boolean;
}

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