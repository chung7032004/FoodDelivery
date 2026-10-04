import { ReactNode } from 'react';
import { useQuery } from '@tanstack/react-query';
import { Navigate } from 'react-router-dom';
import { useAuth } from '@/contexts/AuthContext';
import { restaurantService } from '@/services/restaurant.service';

export function RestaurantApprovalRoute({ children }: { children: ReactNode }) {
  const { user, isAuthReady } = useAuth();
  const isRestaurantStaff = user?.roles.some((role) => role.toLowerCase() === 'restaurantstaff') ?? false;
  const applicationQuery = useQuery({
    queryKey: ['my-restaurant-application'],
    queryFn: restaurantService.getMyApplication,
    enabled: isAuthReady && !isRestaurantStaff,
  });

  if (!isAuthReady || (!isRestaurantStaff && applicationQuery.isLoading)) {
    return <div className="flex min-h-screen items-center justify-center text-slate-500">Đang kiểm tra trạng thái nhà hàng...</div>;
  }

  if (isRestaurantStaff) return <>{children}</>;

  if (applicationQuery.isError || !applicationQuery.data?.isSuccess) {
    return <div role="alert" className="flex min-h-screen flex-col items-center justify-center gap-3 px-4 text-center text-sm text-red-700">
      <p>Không kiểm tra được trạng thái phê duyệt nhà hàng.</p>
      <button type="button" onClick={() => void applicationQuery.refetch()} className="font-semibold underline">Thử lại</button>
    </div>;
  }

  const applicationStatus = applicationQuery.data.data?.application?.applicationStatus;
  if (applicationStatus !== 1 && String(applicationStatus) !== '1' && String(applicationStatus) !== 'Approved') {
    return <Navigate to="/restaurant/apply" replace />;
  }

  return <>{children}</>;
}