import { useState } from 'react';
import type { Dish } from '../../types/index';
import { Badge, StarRating } from '../ui';
import Skeleton from '../ui/Skeleton.tsx';
import { mapProductToDish } from '@/utils/mappers.ts';
import { productService } from '@/services/product.service.ts';
import { useQuery } from '@tanstack/react-query';
import { getImageUrl } from '@/utils/image.ts';
import { useNavigate } from 'react-router-dom';
import { Utensils } from 'lucide-react';

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
      onClick={() => navigate(`/product/${dish.id}`)}
      className="bg-white rounded-2xl overflow-hidden cursor-pointer group transition-all duration-200 hover:-translate-y-1 hover:shadow-xl shadow-sm">

      {/* Thumbnail */}
      <div
        className="relative h-40 flex items-center justify-center"
        style={{ background: dish.bgGradient ?? 'linear-gradient(135deg,#FFF0E8,#FFE0CC)' }}
      >
        <span className="text-7xl leading-none group-hover:scale-105 transition-transform duration-300">
          {/* {dish.emoji} */}
          {dish.mainImageUrl ? (
            <img
              src={getImageUrl(dish.mainImageUrl)}
              alt={dish.name}
              className="w-full h-full object-cover"
              onError={(e) => { (e.target as HTMLImageElement).src = '/placeholder-food.png'; }}
            />
          ) : (
            <Utensils size={48} className="text-[#fb6900]/30" />
          )}
        </span>

        {/* Tag badge */}
        {dish.tag && <Badge label={dish.tag} color={dish.tagColor} />}

        {/* Favorite button */}
        <button
          aria-label="Yêu thích"
          onClick={(e) => { e.stopPropagation(); setFavorite(!favorite); }}
          className="absolute top-3 right-3 bg-white w-8 h-8 rounded-full flex items-center justify-center shadow-md text-sm hover:scale-110 transition-transform cursor-pointer"
        >
          {favorite ? '❤️' : '🤍'}
        </button>
      </div>

      {/* Content */}
      <div className="p-4">
        <h3 className="font-display font-semibold text-[15px] text-gray-900 mb-1">
          {dish.name}
        </h3>
        <p className="text-xs text-gray-400 mb-3">
          🏠 {dish.restaurant} · {dish.distance}
        </p>

        {/* Footer: price + rating + add btn */}
        <div className="flex items-center justify-between">
          <span className="font-display text-lg font-bold text-brand">
            {formatPrice(dish.price)}
          </span>

          <div className="flex items-center gap-2">
            <StarRating rating={0} reviewCount={dish.reviewCount} />
            <button
              aria-label="Thêm vào giỏ"
              onClick={() => onAddToCart?.(dish)}
              className="bg-brand text-white w-7 h-7 rounded-full flex items-center justify-center text-lg font-medium cursor-pointer hover:bg-brand-dark hover:scale-110 active:scale-95 transition-all"
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
    queryKey: ['products', { page: 1, pageSize: 10 }],
    queryFn: async () => {
      const res = await productService.getAll({ page: 1, pageSize: 8 });
      var data = res.data.data.map(mapProductToDish);
      console.log(res)
      return data  // res.data = PagedResponse, .data = mảng product
    },
  });

  return (
    <section className="px-8 pb-10 max-w-7xl mx-auto">
      <div className="flex items-baseline justify-between mb-7">
        <h2 className="font-display text-2xl font-bold tracking-tight">Món nổi bật hôm nay</h2>
        <a href="#" className="text-sm font-medium text-brand hover:underline">
          Xem tất cả →
        </a>
      </div>

      {isLoading ? (
        <div className="grid grid-cols-3 gap-5">
          {Array.from({ length: 6 }).map((_, i) => (
            <Skeleton key={i} shape="rect" height={220} className="rounded-2xl" />
          ))}
        </div>
      ) : error ? (
        <p className="text-sm text-red-500">Không tải được danh sách món ăn.</p>
      ) : (
        <div className="grid grid-cols-3 gap-5">
          {data?.map((dish) => (
            <DishCard key={dish.id} dish={dish} onAddToCart={onAddToCart} />
          ))}
        </div>
      )}
    </section>
  );
}