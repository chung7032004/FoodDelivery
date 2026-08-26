// src/routes/AppRoutes.tsx
import { Routes, Route, Navigate } from 'react-router-dom';
import { HomePage } from '../pages/HomePage';
import { LoginPage } from '../pages/LoginPage';
import { RegisterPage } from '../pages/RegisterPage';
import  { ProtectedRoute}  from './ProtectedRoute';
import { AccountPage } from '../pages/AccountPage';
import { SendOtpPage } from '@/pages/SendOtpPage';
import { ResetPasswordPage } from '@/pages/ResetPasswordPage';
import { ListCategoryPage } from '@/pages/ListCategoryPage';
import ProductDetailPage from '@/pages/ProductDetailPage';
import { ProductListPage } from '@/pages/ProductListPage';
import AdminHomePage from '@/pages/AdminHomePage';


export function AppRoutes() {
  return (
    <Routes>
      <Route path="/" element={<HomePage />} />
      <Route path="/login" element={<LoginPage />} />
      <Route path="/register" element={<RegisterPage />} />
      <Route path="/send-otp" element={<SendOtpPage />} />
      <Route path="/reset-password" element={<ResetPasswordPage />} />
      <Route path= "/list-category" element={<ListCategoryPage />} />
      <Route path = "/product/:productId" element={<ProductDetailPage />} />
      <Route path="/products" element={<ProductListPage />} />
      {/* Route cần đăng nhập mới vào được */}
      <Route
        path="/account"
        element={
          <ProtectedRoute>
            <AccountPage />
          </ProtectedRoute>
        }
      />
      {/* Admin */}
      <Route
        path="/admin"
        element={<AdminHomePage/>}/>
      {/* 404 — không khớp route nào thì quay về Home */}
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}