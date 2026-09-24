// src/routes/ProtectedRoute.tsx
import { ReactNode } from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '@/contexts/AuthContext';

interface ProtectedRouteProps {
  children: ReactNode;
  requiredRoles?: string[];
}

export function ProtectedRoute({ children, requiredRoles = [] }: ProtectedRouteProps) {
  const { user, isAuthenticated, isAuthReady } = useAuth();

  if (!isAuthReady) {
    return <div className="flex min-h-screen items-center justify-center text-slate-500">Đang xác thực...</div>;
  }

  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  const userRoles = user?.roles.map((role) => role.trim().toLowerCase()) ?? [];
  const hasRequiredRole = requiredRoles.length === 0
    || requiredRoles.some((role) => userRoles.includes(role.trim().toLowerCase()));

  if (!hasRequiredRole) {
    return <Navigate to="/" replace />;
  }

  return <>{children}</>;
}