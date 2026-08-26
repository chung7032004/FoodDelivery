import { jwtDecode } from 'jwt-decode';
const ROLE_CLAIM = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
interface AccessTokenPayload {
  sub: string;
  name: string;
  jti: string;
  exp: number;
  iss: string;
  aud: string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'?: string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'?: string;
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'?: string | string[];
}

export function decodeAccessToken(token: string): AccessTokenPayload | null {
  try {
    return jwtDecode<AccessTokenPayload>(token);
  } catch {
    return null;
  }
}
export function isTokenExpired(payload: { exp: number }): boolean {
  return Date.now() >= payload.exp * 1000;
}
export function getRolesFromPayload (payload: AccessTokenPayload | null): string[] {
  if(!payload) return []
  const raw = payload[ROLE_CLAIM];
  if (!raw) return [];
  return Array.isArray(raw) ? raw : [raw];
}
export function getRolesFromToken(token: string): string[] {
  return getRolesFromPayload(decodeAccessToken(token));
}