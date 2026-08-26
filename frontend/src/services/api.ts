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

const ACCESS_TOKEN_KEY = 'accessToken';

let accessToken: string | null = null;
type Listener = (token: string | null) => void;
const listeners = new Set<Listener>();

function readStoredToken(): string | null {
  if (typeof window === 'undefined') return null;
  return window.localStorage.getItem(ACCESS_TOKEN_KEY);
}

export const tokenStore = {
  get: () => accessToken ?? readStoredToken(),
  set: (token: string | null) => {
    accessToken = token;
    if (typeof window !== 'undefined') {
      if (token) {
        window.localStorage.setItem(ACCESS_TOKEN_KEY, token);
      } else {
        window.localStorage.removeItem(ACCESS_TOKEN_KEY);
      }
    }
    listeners.forEach((listener) => listener(token));
  },
  clear: () => {
    accessToken = null;
    if (typeof window !== 'undefined') {
      window.localStorage.removeItem(ACCESS_TOKEN_KEY);
    }
    listeners.forEach((listener) => listener(null));
  },
  subscribe: (listener: Listener) => {
    listeners.add(listener);
    return () => { listeners.delete(listener); };
  }
};

// ---- Request: gắn access token ----
api.interceptors.request.use((config) => {
  const token = tokenStore.get();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
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
      originalRequest.url?.includes('/auth/login') ||
      originalRequest.url?.includes('/auth/logout')
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

let bootstrapPromise: Promise<boolean> | null = null;

export async function bootstrapSession(): Promise<boolean> {
  if (bootstrapPromise) return bootstrapPromise;
  
  bootstrapPromise = (async () => {
    try {
      const { data } = await axios.post<{ accessToken: string }>(
        `${import.meta.env.VITE_API_URL}/auth/refresh`,
        {},
        { withCredentials: true }
      );
      tokenStore.set(data.accessToken);
      return true;
    } catch (error) {
      console.warn('Bootstrap session failed:', error);
      tokenStore.clear();
      return false;
    }
    finally {
      bootstrapPromise = null;
    }
  })();
  return bootstrapPromise;
}