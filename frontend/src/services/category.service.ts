import { ApiResult } from "@/types"
import { api } from "./api"
import { PagedResponse } from "@/types/common"
import { CategoryQueryParams } from "@/types/category"

export interface CategoryResponse {
    id : string
    name : string
    description : string
    displayOrder : number
    imageUrl : string | null
    createdAt : string // ISO string, parse bằng new Date() khi cần
}
export const categoryService = {
    getAll : async (params : CategoryQueryParams) : Promise<ApiResult<PagedResponse<CategoryResponse>>> =>{
        const {data} = await api.get<ApiResult<PagedResponse<CategoryResponse>>>("/categories", {params});
        return data;
    }
} 