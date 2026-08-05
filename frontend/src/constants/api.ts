// API endpoints
export const API_ENDPOINTS = {
  AUTH: {
    LOGIN: '/auth/login',
    REGISTER: '/auth/register',
    LOGOUT: '/auth/logout',
    REFRESH: '/auth/refresh',
  },
  PRODUCTS: {
    LIST: '/products',
    GET: (id: string) => `/products/${id}`,
    CREATE: '/products',
    UPDATE: (id: string) => `/products/${id}`,
    DELETE: (id: string) => `/products/${id}`,
  },
  RESTAURANTS: {
    LIST: '/restaurants',
    GET: (id: string) => `/restaurants/${id}`,
  },
  ORDERS: {
    LIST: '/orders',
    GET: (id: string) => `/orders/${id}`,
    CREATE: '/orders',
  },
}
