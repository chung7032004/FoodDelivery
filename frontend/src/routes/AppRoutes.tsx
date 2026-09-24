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

import AdminUsersPage from '@/pages/Admin/AdminUsersPage';
import AdminOrdersPage from '@/pages/Admin/AdminOrdersPage';
import AdminReviewsPage from '@/pages/Admin/AdminReviewsPage';
import AdminShippersPage from '@/pages/Admin/AdminShippersPage';
import AdminCategoriesPage from '@/pages/Admin/AdminCategoriesPage';
import AdminDashboardPage from '@/pages/Admin/AdminDashboardPage';
import AdminProductsPage from '@/pages/Admin/AdminProductsPage';
import RestaurantManagementPage from '@/pages/Restaurant/RestaurantManagementPage';
import RestaurantOrdersPage from '@/pages/Restaurant/RestaurantOrdersPage';
import ShipperHomePage from '@/pages/Shipper/ShipperHomePage';
import ShipperOrdersPage from '@/pages/Shipper/ShipperOrdersPage';
import ShipperAssignedPage from '@/pages/Shipper/ShipperAssignedPage';
import ShipperProfilePage from '@/pages/Shipper/ShipperProfilePage';



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
        element={
          <ProtectedRoute requiredRoles={['Admin']}>
            <AdminDashboardPage />
          </ProtectedRoute>
        }
      />
      <Route path="/admin/users" element={<ProtectedRoute requiredRoles={['Admin']}><AdminUsersPage /></ProtectedRoute>} />
      <Route path="/admin/orders" element={<ProtectedRoute requiredRoles={['Admin']}><AdminOrdersPage /></ProtectedRoute>} />
      <Route path="/admin/shippers" element={<ProtectedRoute requiredRoles={['Admin']}><AdminShippersPage /></ProtectedRoute>} />
      <Route path="/admin/reviews" element={<ProtectedRoute requiredRoles={['Admin']}><AdminReviewsPage /></ProtectedRoute>} />
      <Route path="/admin/categories" element={<ProtectedRoute requiredRoles={['Admin']}><AdminCategoriesPage /></ProtectedRoute>} />
      <Route path="/admin/products" element={<ProtectedRoute requiredRoles={['Admin']}><AdminProductsPage /></ProtectedRoute>} />
      <Route path="/restaurant" element={<ProtectedRoute requiredRoles={['RestaurantOwner', 'RestaurantStaff']}><RestaurantManagementPage /></ProtectedRoute>} />
      <Route path="/restaurant/orders" element={<ProtectedRoute requiredRoles={['RestaurantOwner', 'RestaurantStaff']}><RestaurantOrdersPage /></ProtectedRoute>} />
      <Route path="/restaurant/profile" element={<ProtectedRoute requiredRoles={['RestaurantOwner', 'RestaurantStaff']}><RestaurantManagementPage /></ProtectedRoute>} />
      <Route path="/restaurant/menu" element={<ProtectedRoute requiredRoles={['RestaurantOwner', 'RestaurantStaff']}><AdminProductsPage workspace="restaurant" /></ProtectedRoute>} />
      <Route path="/shipper" element={<ProtectedRoute requiredRoles={['Shipper']}><ShipperHomePage /></ProtectedRoute>} />
      <Route path="/shipper/orders" element={<ProtectedRoute requiredRoles={['Shipper']}><ShipperOrdersPage /></ProtectedRoute>} />
      <Route path="/shipper/assigned" element={<ProtectedRoute requiredRoles={['Shipper']}><ShipperAssignedPage /></ProtectedRoute>} />
      <Route path="/shipper/profile" element={<ProtectedRoute requiredRoles={['Shipper']}><ShipperProfilePage /></ProtectedRoute>} />
      {/* 404 — không khớp route nào thì quay về Home */}
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}