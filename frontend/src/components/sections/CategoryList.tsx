import { Link } from 'react-router-dom';
import { ChartBarStacked, ChevronRight } from 'lucide-react';
import { getImageUrl } from '@/utils/image';
import type { Category } from '@/types/category';
import { cn } from '@/utils/cn';

interface CategoryListProps {
  categories: Category[];
  selectedCategoryId: string | null;
  onSelectCategory: (id: string | null) => void;
}

export function CategoryList({ categories, selectedCategoryId, onSelectCategory }: CategoryListProps) {
  return (
    <section aria-labelledby="home-categories-heading" className="mx-auto max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
      <div className="mb-5 flex items-end justify-between gap-4 sm:mb-6">
        <div>
          <p className="text-xs font-bold uppercase tracking-[0.16em] text-orange-600">Chọn theo khẩu vị</p>
          <h2 id="home-categories-heading" className="mt-1 font-display text-xl font-bold tracking-tight text-slate-950 sm:text-2xl">
            Danh mục món ăn
          </h2>
        </div>
        <Link
          to="/list-category"
          className="inline-flex shrink-0 items-center gap-1 text-sm font-semibold text-orange-600 transition hover:text-orange-700"
        >
          Tất cả danh mục <ChevronRight size={16} />
        </Link>
      </div>

      <div className="flex gap-3 overflow-x-auto pb-2 [scrollbar-width:thin]">
        <button
          type="button"
          aria-pressed={selectedCategoryId === null}
          onClick={() => onSelectCategory(null)}
          className={cn(
            'flex min-w-[92px] shrink-0 flex-col items-center gap-2 rounded-2xl border px-4 py-3 transition',
            selectedCategoryId === null
              ? 'border-orange-500 bg-orange-50 shadow-sm'
              : 'border-slate-200 bg-white hover:border-orange-200 hover:bg-orange-50/50'
          )}
        >
          <span className={cn(
            'flex h-11 w-11 items-center justify-center rounded-xl',
            selectedCategoryId === null ? 'bg-orange-500 text-white' : 'bg-orange-50 text-orange-600'
          )}>
            <ChartBarStacked size={20} />
          </span>
          <span className="text-xs font-semibold text-slate-800">Tất cả</span>
        </button>

        {categories.map((category) => {
          const isSelected = selectedCategoryId === category.id;
          return (
            <button
              key={category.id}
              type="button"
              aria-pressed={isSelected}
              onClick={() => onSelectCategory(isSelected ? null : category.id)}
              className={cn(
                'group flex min-w-[92px] shrink-0 flex-col items-center gap-2 rounded-2xl border px-4 py-3 transition',
                isSelected
                  ? 'border-orange-500 bg-orange-50 shadow-sm'
                  : 'border-slate-200 bg-white hover:border-orange-200 hover:bg-orange-50/50'
              )}
            >
              <span className={cn(
                'flex h-11 w-11 items-center justify-center overflow-hidden rounded-xl transition-transform group-hover:scale-105',
                isSelected ? 'bg-orange-100 text-orange-600' : 'bg-slate-100 text-slate-500'
              )}>
                {category.imageUrl ? (
                  <img
                    src={getImageUrl(category.imageUrl)}
                    alt=""
                    loading="lazy"
                    className="h-full w-full object-cover"
                  />
                ) : (
                  <ChartBarStacked size={20} />
                )}
              </span>
              <span className="max-w-24 truncate text-xs font-semibold text-slate-800">{category.name}</span>
            </button>
          );
        })}
      </div>
    </section>
  );
}
