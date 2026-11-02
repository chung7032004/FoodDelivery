import { Link } from 'react-router-dom';
import { ChartBarStacked } from 'lucide-react';
import { useEffect, useState } from 'react';
import { getImageUrl } from '@/utils/image';
import type { Category } from '@/types/category';
import { cn } from '@/utils/cn';

interface CategoryListProps {
  categories: Category[];
  /** Số món mỗi category — key = category.id. Nếu không truyền, ẩn dòng "x món". */
  productCounts?: Record<string, number>;
}

// ─── CategoryItem ─────────────────────────────────────────────────
interface CategoryItemProps {
  category: Category;
  isActive: boolean;
  productCount?: number;
  onClick: (id: string) => void;
}

function CategoryItem({ category, isActive, productCount, onClick }: CategoryItemProps) {
  return (
    <button
      type="button"
      onClick={() => onClick(category.id)}
      aria-pressed={isActive}
      className={cn(
        'group flex cursor-pointer flex-col items-center gap-2 rounded-2xl border-[1.5px] bg-white p-3 sm:p-4',
        'transition-all duration-200',
        isActive
          ? 'border-brand bg-orange-50'
          : 'border-transparent hover:border-brand/30 hover:bg-orange-50/50'
      )}
    >
      {/* Icon / Image */}
      {category.imageUrl ? (
        <img
          src={getImageUrl(category.imageUrl)}
          alt={category.name}
          className="h-10 w-10 rounded object-cover transition-transform duration-200 group-hover:scale-105 sm:h-11 sm:w-11"
        />
      ) : (
        <span className="flex h-10 w-10 shrink-0 items-center justify-center rounded-xl bg-[#fb6900]/10 text-[#fb6900] transition-transform duration-200 group-hover:scale-105 sm:h-11 sm:w-11">
          <ChartBarStacked size={20} />
        </span>
      )}

      {/* Name */}
      <span className="line-clamp-1 w-full text-center text-xs font-medium text-gray-900 sm:text-sm">
        {category.name}
      </span>

      {/* Count — chỉ hiện nếu có dữ liệu */}
      {typeof productCount === 'number' && (
        <span className="text-[10px] text-gray-400 sm:text-xs">
          {productCount} món
        </span>
      )}
    </button>
  );
}

// ─── CategoryList ─────────────────────────────────────────────────
export function CategoryList({ categories, productCounts }: CategoryListProps) {
  const [activeId, setActiveId] = useState<string | null>(
    categories.length ? categories[0].id : null
  );

  // Đồng bộ activeId khi categories thay đổi (fetch lại, filter...)
  useEffect(() => {
    if (!categories.length) {
      setActiveId(null);
      return;
    }
    const stillExists = categories.some((c) => c.id === activeId);
    if (!stillExists) {
      setActiveId(categories[0].id);
    }
  }, [categories, activeId]);

  if (!categories.length) return null;

  return (
    <section className="mx-auto max-w-7xl px-4 py-8 sm:px-6 sm:py-10 lg:px-8">
      {/* Section header */}
      <div className="mb-5 flex items-baseline justify-between sm:mb-7">
        <h2 className="font-display text-xl font-bold tracking-tight sm:text-2xl">
          Danh mục món
        </h2>
        <Link
          to="/list-category"
          className="text-sm font-medium text-brand hover:underline"
        >
          Xem tất cả →
        </Link>
      </div>

      {/* Category grid — responsive */}
      <div className="grid grid-cols-3 gap-2.5 sm:grid-cols-4 sm:gap-3 md:grid-cols-5">
        {categories.map((cat) => (
          <CategoryItem
            key={cat.id}
            category={cat}
            isActive={activeId === cat.id}
            productCount={productCounts?.[cat.id]}
            onClick={setActiveId}
          />
        ))}
      </div>
    </section>
  );
}