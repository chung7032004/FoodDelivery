import axios, { AxiosError, InternalAxiosRequestConfig } from "axios";

export const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
  withCredentials: true // bắt buộc để gửi / nhận cookie từ server
});

export interface ApiErrorResponse {
  isSuccess: boolean;
  errorCode: string | null;
  message: string | null;
  statusCode: number;
}

let accessToken: string | null = null;

export const tokenStore = {
  get: () => accessToken,
  set: (token: string | null) => {
    accessToken = token;
  },
  clear: () => {
    accessToken = null;
  }
}

// ---- Request: gắn access token ----
api.interceptors.request.use((config) => {
  if (accessToken) {
    config.headers.Authorization = `Bearer ${accessToken}`;
  }
  return config;
});

// Refresh queue
let isRefreshing = false;
let subscribers: ((token: string) => void)[] = [];

function subscribeTokenRefresh(cb: (token: string) => void) {
  subscribers.push(cb);
}

function onRefreshed(token: string) {
  subscribers.forEach((cb) => cb(token));
  subscribers = [];
}

function onRefreshFailed() {
  subscribers = [];
  tokenStore.clear();
  // Chỉ redirect nếu chưa ở trang login
  if (!window.location.pathname.includes('/login')) {
    window.location.href = "/login";
  }
}

export interface ApiError {
    errorCode: string;
  message: string;
  statusCode: number;
}

function toApiError(error: unknown): ApiError {
  if (axios.isAxiosError<ApiErrorResponse>(error)) {
    return {
      errorCode: error.response?.data?.errorCode ?? "UNKNOWN_ERROR",
      message: error.response?.data?.message ?? "Có lỗi xảy ra, vui lòng thử lại.",
      statusCode: error.response?.data?.statusCode ?? error.response?.status ?? 500,
    };
  }
  return {
    errorCode: "UNKNOWN_ERROR",
    message: "Có lỗi xảy ra, vui lòng thử lại.",
    statusCode: 500,
  };
}

interface RetryConfig extends InternalAxiosRequestConfig {
  _retry?: boolean;
}

api.interceptors.response.use(
  (response) => response,
  async (error: AxiosError<ApiErrorResponse>) => {
    const originalRequest = error.config as RetryConfig;
    
    // Không xử lý refresh cho các request đặc biệt
    if (
      !originalRequest ||
      error.response?.status !== 401 ||
      originalRequest.url?.includes('/auth/refresh') ||
      originalRequest.url?.includes('/auth/login')
    ) {
      return Promise.reject(toApiError(error));
    }

    // Nếu đã retry rồi thì fail
    if (originalRequest._retry) {
      onRefreshFailed();
      return Promise.reject(toApiError(error));
    }

    originalRequest._retry = true;

    // Nếu đang refresh thì đợi
    if (isRefreshing) {
      try {
        const newToken = await new Promise<string>((resolve, reject) => {
          subscribeTokenRefresh(resolve);
          // Timeout để tránh treo
          setTimeout(() => reject(new Error('Refresh timeout')), 10000);
        });
        originalRequest.headers.Authorization = `Bearer ${newToken}`;
        return api(originalRequest);
      } catch {
        return Promise.reject(toApiError(error));
      }
    }

    isRefreshing = true;

    try {
      const { data } = await axios.post<{ accessToken: string }>(
        `${import.meta.env.VITE_API_URL}/auth/refresh`,
        {},
        { withCredentials: true }
      );
      
      tokenStore.set(data.accessToken);
      isRefreshing = false;
      onRefreshed(data.accessToken);
      
      originalRequest.headers.Authorization = `Bearer ${data.accessToken}`;
      return api(originalRequest);
    } catch (refreshError) {
      isRefreshing = false;
      onRefreshFailed();
      return Promise.reject(toApiError(refreshError));
    }
  }
);