import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import { Utensils } from 'lucide-react';
import type { Dish } from '@/types';
import { Badge, StarRating } from '../ui';
import Skeleton from '../ui/Skeleton';
import { mapProductToDish } from '@/utils/mappers';
import { productService } from '@/services/product.service';
import { getImageUrl } from '@/utils/image';
import { cn } from '@/utils/cn';

// ─── formatPrice ─────────────────────────────────────────────────
function formatPrice(price: number): string {
  return price.toLocaleString('vi-VN') + 'đ';
}

// ─── DishCard ─────────────────────────────────────────────────────
interface DishCardProps {
  dish: Dish;
  onAddToCart?: (dish: Dish) => void;
}

export function DishCard({ dish, onAddToCart }: DishCardProps) {
  const [favorite, setFavorite] = useState(dish.isFavorite ?? false);
  const navigate = useNavigate();

  return (
    <article
      role="link"
      tabIndex={0}
      onClick={() => navigate(`/product/${dish.id}`)}
      onKeyDown={(e) => {
        if (e.key === 'Enter' || e.key === ' ') {
          e.preventDefault();
          navigate(`/product/${dish.id}`);
        }
      }}
      className={cn(
        'group cursor-pointer overflow-hidden rounded-2xl bg-white shadow-sm',
        'transition-all duration-200 hover:-translate-y-1 hover:shadow-xl',
        'focus:outline-none focus-visible:ring-2 focus-visible:ring-brand/50'
      )}
    >
      {/* Thumbnail */}
      <div
        className="relative h-40 overflow-hidden"
        style={{ background: dish.bgGradient ?? 'linear-gradient(135deg,#FFF0E8,#FFE0CC)' }}
      >
        {dish.mainImageUrl ? (
          <img
            src={getImageUrl(dish.mainImageUrl)}
            alt={dish.name}
            loading="lazy"
            className="h-full w-full object-cover transition-transform duration-300 group-hover:scale-105"
            onError={(e) => {
              const img = e.currentTarget;
              img.onerror = null; // tránh loop
              img.src = '/placeholder-food.png';
            }}
          />
        ) : (
          <div className="flex h-full w-full items-center justify-center">
            <Utensils size={48} className="text-[#fb6900]/30" />
          </div>
        )}

        {/* Tag badge */}
        {dish.tag && <Badge label={dish.tag} color={dish.tagColor} />}

        {/* Favorite button */}
        <button
          type="button"
          aria-label={favorite ? 'Bỏ yêu thích' : 'Yêu thích'}
          aria-pressed={favorite}
          onClick={(e) => {
            e.stopPropagation();
            setFavorite((v) => !v);
          }}
          className="absolute right-3 top-3 flex h-8 w-8 items-center justify-center rounded-full bg-white text-sm shadow-md transition-transform hover:scale-110 active:scale-95"
        >
          {favorite ? '❤️' : '🤍'}
        </button>
      </div>

      {/* Content */}
      <div className="p-3 sm:p-4">
        <h3 className="mb-1 line-clamp-1 font-display text-sm font-semibold text-gray-900 sm:text-[15px]">
          {dish.name}
        </h3>
        <p className="mb-3 line-clamp-1 text-xs text-gray-400">
          🏠 {dish.restaurant}
          {dish.distance && <> · {dish.distance}</>}
        </p>

        {/* Footer */}
        <div className="flex items-center justify-between gap-2">
          <span className="font-display text-base font-bold text-brand sm:text-lg">
            {formatPrice(dish.price)}
          </span>

          <div className="flex items-center gap-1.5 sm:gap-2">
            <StarRating rating={dish.rating ?? 0} reviewCount={dish.reviewCount} />
            <button
              type="button"
              aria-label="Thêm vào giỏ"
              onClick={(e) => {
                e.stopPropagation();
                onAddToCart?.(dish);
              }}
              className="flex h-7 w-7 items-center justify-center rounded-full bg-brand text-lg font-medium text-white transition-all hover:scale-110 hover:bg-brand-dark active:scale-95"
            >
              +
            </button>
          </div>
        </div>
      </div>
    </article>
  );
}

// ─── DishGrid ─────────────────────────────────────────────────────
interface DishGridProps {
  onAddToCart?: (dish: Dish) => void;
}

export function DishGrid({ onAddToCart }: DishGridProps) {
  const { data, isLoading, error } = useQuery({
    queryKey: ['products', { page: 1, pageSize: 8 }],
    queryFn: async () => {
      const res = await productService.getAll({ page: 1, pageSize: 8 });
      return res.data.data.map(mapProductToDish);
    },
  });

  return (
    <section className="mx-auto max-w-7xl px-4 pb-10 sm:px-6 lg:px-8">
      <div className="mb-5 flex items-baseline justify-between sm:mb-7">
        <h2 className="font-display text-xl font-bold tracking-tight sm:text-2xl">
          Món nổi bật hôm nay
        </h2>
        <Link to="/products" className="text-sm font-medium text-brand hover:underline">
          Xem tất cả →
        </Link>
      </div>

      {isLoading ? (
        <div className="grid grid-cols-2 gap-3 sm:grid-cols-3 sm:gap-5 lg:grid-cols-4">
          {Array.from({ length: 8 }).map((_, i) => (
            <Skeleton key={i} shape="rect" height={220} className="rounded-2xl" />
          ))}
        </div>
      ) : error ? (
        <p className="text-sm text-red-500">Không tải được danh sách món ăn.</p>
      ) : !data?.length ? (
        <p className="text-sm text-gray-500">Chưa có món nào.</p>
      ) : (
        <div className="grid grid-cols-2 gap-3 sm:grid-cols-3 sm:gap-5 lg:grid-cols-4">
          {data.map((dish) => (
            <DishCard key={dish.id} dish={dish} onAddToCart={onAddToCart} />
          ))}
        </div>
      )}
    </section>
  );
}