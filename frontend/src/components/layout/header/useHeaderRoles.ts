import { useAuth } from '@/contexts/AuthContext';

export function useHeaderRoles() {
  const { user } = useAuth();
  const roles = user?.roles ?? [];

  return {
    user,
    isAdmin: roles.includes('Admin'),
    canManageRestaurant: roles.includes('RestaurantOwner') || roles.includes('RestaurantStaff'),
    isShipper: roles.includes('Shipper'),
  };
}