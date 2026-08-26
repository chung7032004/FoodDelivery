// src/contexts/AuthContext.tsx
import { createContext, useContext, useState, useEffect, useMemo, useCallback } from 'react';
import { bootstrapSession, tokenStore } from '@/services/api';
import { decodeAccessToken, getRolesFromPayload, isTokenExpired } from '@/utils/jwt';
import { authService } from '@/services/auth.service';

interface AuthUser {
  id: string;
  name: string;
  email?: string;
  roles: string[];
}

interface AuthContextValue {
  user: AuthUser | null;
  isAuthenticated: boolean;
  isAuthReady: boolean;
  logout: () => Promise<void>;
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [accessToken, setAccessTokenState] = useState<string | null>(tokenStore.get());
  const [isAuthReady, setIsAuthReady] = useState(false);
  const [bootstrapAttempted, setBootstrapAttempted] = useState(false);
  let mounted = true;
  useEffect(() => {
    (async () => {
      if (bootstrapAttempted) return;
      setBootstrapAttempted(true);
      const success = await bootstrapSession();
      setIsAuthReady(true);
      if(mounted){
        setIsAuthReady(true);
        if (!success) {
          const currentToken = tokenStore.get();
          if (currentToken) {
            const payload = decodeAccessToken(currentToken);
            if (payload && isTokenExpired(payload)) {
              tokenStore.clear();
            }
          }
        }
      }
    })();
    return () => {
        mounted = false;
      };
    }, [bootstrapAttempted]);
  useEffect(() => {
    const unsubscribe = tokenStore.subscribe((token) => {
      setAccessTokenState(token);
    });
    return unsubscribe;
  }, []);
  const user = useMemo<AuthUser | null>(() => {
    if (!accessToken) return null;
    const payload = decodeAccessToken(accessToken);
    if (!payload || isTokenExpired(payload)) return null;
    return {
      id: payload.sub,
      name: payload.name,
      email: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
      roles: getRolesFromPayload(payload),
    };
  }, [accessToken]);

  const logout = useCallback(async () => {
     await authService.logout();
  }, []);

  const value: AuthContextValue = {
    user,
    isAuthenticated: !!user,
    isAuthReady,
    logout,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth must be used within AuthProvider');
  return ctx;
}