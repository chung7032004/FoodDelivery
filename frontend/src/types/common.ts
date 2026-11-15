export interface PaginationMeta {
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

export interface PagedResponse<T> {
  data: T[];
  meta: PaginationMeta;
}

// Alias for compatibility
export type PagedResult<T> = PagedResponse<T>;

export interface ApiResult<T = any> {
  isSuccess: boolean;
  data: T;
  errorCode: string | null;
  message: string | null;
  statusCode: number;
}

export interface ApiErrorResponse {
  isSuccess: boolean;
  errorCode: string | null;
  message: string | null;
  statusCode: number;
}

export interface ApiError {
  errorCode: string;
  message: string;
  statusCode: number;
}

export interface Coordinates {
  lat: number;
  lng: number;
}
