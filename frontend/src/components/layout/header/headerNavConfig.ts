import { ShieldCheck, Store, Package } from 'lucide-react';
import type { RoleLink } from '@/types';

export type { RoleLink };

export const NAV_LINKS = [
  { label: 'Trang chủ', to: '/' },
  { label: 'Thực đơn', to: '/list-category' },
  { label: 'Nhà hàng', to: '/restaurants' },
  { label: 'Khuyến mãi', to: '/promotions' },
] as const;

export const ROLE_LINKS: Record<'admin' | 'restaurant' | 'shipper', RoleLink> = {
  admin: {
    to: '/admin',
    label: 'Quản trị',
    title: 'Mở trang quản trị',
    icon: ShieldCheck,
    pillClass:
      'border-orange-500/30 bg-orange-500/10 text-orange-600 hover:bg-orange-500 hover:text-white',
    itemClass: 'text-orange-600 hover:bg-orange-500/10',
  },
  restaurant: {
    to: '/restaurant',
    label: 'Quản lý quán',
    title: 'Quản lý nhà hàng',
    icon: Store,
    pillClass:
      'border-sky-500/30 bg-sky-500/10 text-sky-700 hover:bg-sky-500 hover:text-white',
    itemClass: 'text-sky-700 hover:bg-sky-500/10',
  },
  shipper: {
    to: '/shipper',
    label: 'Giao hàng',
    title: 'Mở khu vực giao hàng',
    icon: Package,
    pillClass:
      'border-emerald-500/30 bg-emerald-500/10 text-emerald-700 hover:bg-emerald-600 hover:text-white',
    itemClass: 'text-emerald-700 hover:bg-emerald-500/10',
  },
};