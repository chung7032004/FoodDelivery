export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  fullName: string;
  phone: string;
}

export interface ChangePasswordRequest {
  oldPassword: string;
  newPassword: string;
  confirmNewPassword: string;
}

export interface LoginResponse {
  accessToken: string;
}

export interface AuthUser {
  id: string;
  name: string;
  email?: string;
  roles: string[];
}

export interface AccessTokenPayload {
  sub: string;
  name: string;
  jti?: string;
  exp: number;
  iss?: string;
  aud?: string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'?: string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'?: string;
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'?: string | string[];
  [key: string]: any;
}

export interface AuthContextValue {
  user: AuthUser | null;
  isAuthenticated: boolean;
  isAuthReady: boolean;
  logout: () => Promise<void>;
}

// Form & validation state for Auth pages
export interface LoginFormData {
  email: string;
  password: string;
}

export interface LoginErrors {
  email?: string;
  password?: string;
  form?: string;
}

export interface RegisterFormData {
  fullName: string;
  email: string;
  phone: string;
  password: string;
  confirmPassword: string;
}

export type RegisterErrors = Partial<Record<keyof RegisterFormData, string>> & { form?: string };

export interface ResetPasswordFormData {
  email?: string;
  otp?: string;
  password?: string;
  newPassword?: string;
  confirmPassword?: string;
}

export interface ResetPasswordErrors {
  email?: string;
  otp?: string;
  newPassword?: string;
  confirmNewPassword?: string;
  confirmPassword?: string;
  form?: string;
}
