import { ApiResult } from "@/types"
import { api } from "./api"

export interface CategoryResponse {
    id : string
    name : string
    description : string
    displayOrder : number
}
export const categoryService = {
    getAll : async (): Promise<CategoryResponse[]> =>{
        const res = await api.get<ApiResult<CategoryResponse[]>>("/categories")
        return res.data.data
    }
} 