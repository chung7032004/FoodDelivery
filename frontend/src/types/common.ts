export interface PaginationMeta {
    page : number,
    pageSize : number,
    totalCount : number,
    totalPages : number,
}
export interface PagedResponse<T>{
    data : T[]
    meta : PaginationMeta
}
export interface ApiResult<T>{
    isSuccess : boolean,
    data : T | null,
    errorCode : string | null,
    message : string | null,
    statusCode : number
}