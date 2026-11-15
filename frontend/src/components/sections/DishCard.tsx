import { useEffect, useRef, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { LoaderCircle, RefreshCw, Utensils } from 'lucide-react';
import type { Dish } from '@/types';
import { Badge, StarRating } from '../ui';
import Skeleton from '../ui/Skeleton';
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
        {(dish.category || dish.restaurant || dish.distance) && (
          <p className="mb-3 line-clamp-1 text-xs text-gray-400">
            {dish.category ? `🍽 ${dish.category}` : dish.restaurant ? `🏠 ${dish.restaurant}` : ''}
            {dish.distance && <> · {dish.distance}</>}
          </p>
        )}

        {/* Footer */}
        <div className="flex items-center justify-between gap-2">
          <span className="font-display text-base font-bold text-brand sm:text-lg">
            {formatPrice(dish.price)}
          </span>

          <div className="flex items-center gap-1.5 sm:gap-2">
            {dish.rating && dish.rating > 0 && (
              <StarRating rating={dish.rating} reviewCount={dish.reviewCount} />
            )}
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
  dishes: Dish[];
  isLoading: boolean;
  isFetchingNextPage: boolean;
  hasNextPage: boolean;
  error: boolean;
  onAddToCart?: (dish: Dish) => void;
  onLoadMore: () => void;
  onRetry: () => void;
}

export function DishGrid({
  dishes,
  isLoading,
  isFetchingNextPage,
  hasNextPage,
  error,
  onAddToCart,
  onLoadMore,
  onRetry,
}: DishGridProps) {
  const sentinelRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const sentinel = sentinelRef.current;
    if (!sentinel || !hasNextPage || isLoading || isFetchingNextPage || error) return;

    const observer = new IntersectionObserver(
      ([entry]) => {
        if (entry.isIntersecting) onLoadMore();
      },
      { rootMargin: '360px 0px' }
    );
    observer.observe(sentinel);
    return () => observer.disconnect();
  }, [error, hasNextPage, isFetchingNextPage, isLoading, onLoadMore]);

  return (
    <section id="mon-an" aria-labelledby="home-products-heading" className="mx-auto max-w-7xl scroll-mt-24 px-4 pb-12 sm:px-6 lg:px-8">
      <div className="mb-5 flex items-end justify-between gap-4 sm:mb-7">
        <div>
          <p className="text-xs font-bold uppercase tracking-[0.16em] text-orange-600">Khám phá thực đơn</p>
          <h2 id="home-products-heading" className="mt-1 font-display text-xl font-bold tracking-tight text-slate-950 sm:text-2xl">
            Món ngon dành cho bạn
          </h2>
        </div>
        <p className="hidden text-sm text-slate-500 sm:block">Cuộn xuống để khám phá thêm</p>
      </div>

      {isLoading ? (
        <div className="grid grid-cols-2 gap-3 sm:grid-cols-3 sm:gap-5 lg:grid-cols-4">
          {Array.from({ length: 8 }).map((_, i) => (
            <Skeleton key={i} shape="rect" height={220} className="rounded-2xl" />
          ))}
        </div>
      ) : error && dishes.length === 0 ? (
        <div className="rounded-2xl border border-slate-200 bg-white px-5 py-12 text-center">
          <p className="text-sm text-slate-600">Không tải được danh sách món ăn.</p>
          <button type="button" onClick={onRetry} className="mt-4 inline-flex items-center gap-2 rounded-xl bg-orange-500 px-4 py-2 text-sm font-semibold text-white hover:bg-orange-600">
            <RefreshCw size={15} /> Thử lại
          </button>
        </div>
      ) : dishes.length === 0 ? (
        <div className="rounded-2xl border border-dashed border-slate-300 bg-white px-5 py-12 text-center">
          <Utensils className="mx-auto mb-3 text-slate-300" size={36} />
          <p className="font-semibold text-slate-800">Chưa tìm thấy món phù hợp</p>
          <p className="mt-1 text-sm text-slate-500">Thử chọn danh mục khác hoặc tìm bằng từ khóa khác nhé.</p>
        </div>
      ) : (
        <>
          <div className="grid grid-cols-2 gap-3 sm:grid-cols-3 sm:gap-5 lg:grid-cols-4">
            {dishes.map((dish) => (
              <DishCard key={dish.id} dish={dish} onAddToCart={onAddToCart} />
            ))}
          </div>

          {error && (
            <div role="alert" className="mt-6 flex flex-wrap items-center justify-center gap-3 text-sm text-slate-600">
              <span>Chưa tải được các món tiếp theo.</span>
              <button type="button" onClick={onRetry} className="font-semibold text-orange-600 hover:underline">
                Thử lại
              </button>
            </div>
          )}

          {hasNextPage && (
            <div ref={sentinelRef} className="flex min-h-20 flex-col items-center justify-center gap-3 pt-6">
              {isFetchingNextPage ? (
                <p role="status" className="inline-flex items-center gap-2 text-sm text-slate-500">
                  <LoaderCircle size={18} className="animate-spin text-orange-500" />
                  Đang tải thêm món...
                </p>
              ) : (
                <button
                  type="button"
                  onClick={onLoadMore}
                  className="rounded-full border border-orange-200 bg-white px-5 py-2.5 text-sm font-semibold text-orange-700 shadow-sm transition hover:border-orange-400 hover:bg-orange-50"
                >
                  Tải thêm món
                </button>
              )}
            </div>
          )}
          {!hasNextPage && dishes.length > 0 && (
            <p className="pt-8 text-center text-sm text-slate-400">Bạn đã xem hết thực đơn hiện có.</p>
          )}
        </>
      )}
    </section>
  );
}