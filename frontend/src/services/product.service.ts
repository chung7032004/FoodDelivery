import { ApiResult } from "@/types";
import { PagedResponse } from "@/types/common";
import { ProductDetailResponseDto, ProductQueryParams, ProductResponseDto } from "@/types/product";
import { api } from "./api";

export const productService = {
    getAll : async (params : ProductQueryParams) : Promise<ApiResult<PagedResponse<ProductResponseDto>>> =>{
        const {data} = await api.get<ApiResult<PagedResponse<ProductResponseDto>>>("/products", {params});
        return data;
    },
    getDetailById : async (id : string ) : Promise<ApiResult<ProductDetailResponseDto>> =>{
        const {data} = await api.get<ApiResult<ProductDetailResponseDto>>('/products/' + id);
        return data ;
    }
}