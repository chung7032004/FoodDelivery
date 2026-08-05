import { ApiResult } from "@/types";
import { PagedResponse } from "@/types/common";
import { ProductQueryParams, ProductResponseDto } from "@/types/product";
import { api } from "./api";

export const productService = {
    getAll : async (params : ProductQueryParams) : Promise<ApiResult<PagedResponse<ProductResponseDto>>> =>{
        const {data} = await api.get<ApiResult<PagedResponse<ProductResponseDto>>>("/products", {params});
        return data;
    }
}