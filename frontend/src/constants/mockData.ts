import type { Category, Dish, HeroStat, PromoCard } from '../types';

// ─── Hero ─────────────────────────────────────────────────────────
export const HERO_STATS: HeroStat[] = [
  { value: '500+', label: 'Nhà hàng' },
  { value: "30'",  label: 'Giao nhanh' },
  { value: '4.9★', label: 'Đánh giá' },
];

// ─── Categories ──────────────────────────────────────────────────
export const CATEGORIES: Category[] = [
  { id: 1, name: 'Phở & Bún',  emoji: '🍜', count: 86  },
  { id: 2, name: 'Pizza',      emoji: '🍕', count: 54  },
  { id: 3, name: 'Burger',     emoji: '🍔', count: 72  },
  { id: 4, name: 'Cơm hộp',   emoji: '🍱', count: 120 },
  { id: 5, name: 'Trà & Nước', emoji: '🧋', count: 98  },
];

// ─── Dishes ──────────────────────────────────────────────────────
export const FEATURED_DISHES: Dish[] = [
  {
    id: 1,
    name: 'Phở Bò Đặc Biệt',
    restaurant: 'Phở Hà Nội 1996',
    distance: '2.1km',
    price: 65000,
    rating: 4.9,
    reviewCount: 320,
    emoji: '🍜',
    tag: 'Bán chạy',
    tagColor: '#E8441A',
    bgGradient: 'linear-gradient(135deg,#FFF0E8,#FFE0CC)',
    isFavorite: true,
  },
  {
    id: 2,
    name: 'Burger BBQ Double',
    restaurant: 'Burger Factory',
    distance: '1.8km',
    price: 89000,
    rating: 4.7,
    reviewCount: 215,
    emoji: '🍔',
    tag: 'Mới',
    tagColor: '#FF6B35',
    bgGradient: 'linear-gradient(135deg,#FFF3E0,#FFE0B2)',
    isFavorite: false,
  },
  {
    id: 3,
    name: 'Cơm Gà Xối Mỡ',
    restaurant: 'Cơm Tấm Sài Gòn',
    distance: '0.9km',
    price: 55000,
    rating: 4.8,
    reviewCount: 540,
    emoji: '🍱',
    tag: '-20%',
    tagColor: '#16A34A',
    bgGradient: 'linear-gradient(135deg,#E8F5E9,#C8E6C9)',
    isFavorite: false,
  },
];

// ─── Promos ───────────────────────────────────────────────────────
export const PROMO_SMALL: PromoCard[] = [
  {
    id: 1,
    title: 'Combo Trưa tiết kiệm',
    subtitle: 'Từ 45.000đ · Giao nhanh',
    btnLabel: 'Xem combo',
    bgClass: 'bg-brand',
  },
  {
    id: 2,
    title: 'Tích điểm · Đổi quà',
    subtitle: 'Mỗi đơn tích 1 điểm thưởng',
    btnLabel: 'Tham gia',
    bgClass: 'bg-green',
  },
];