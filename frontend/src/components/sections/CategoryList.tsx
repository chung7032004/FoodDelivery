import { Category } from '@/types/category';
import { ChartBarStacked } from 'lucide-react';
import { useState } from 'react';

interface CategoryListProps {
  categories: Category[];
}

// ─── CategoryItem ─────────────────────────────────────────────────
interface CategoryItemProps {
  category: Category;
  isActive: boolean;
  onClick: (id: string) => void;
}

function CategoryItem({ category, isActive, onClick }: CategoryItemProps) {
  return (
    <button
      onClick={() => onClick(category.id)}
      className={`
        flex flex-col items-center gap-2 p-5 rounded-2xl border-[1.5px] cursor-pointer
        transition-all duration-200 bg-white
        ${isActive
          ? 'border-brand bg-orange-50'
          : 'border-transparent hover:border-brand/30 hover:bg-orange-50/50'
        }
      `}
    >
      <span className="text-4xl">
        {category.imageUrl ? (
          <img src={category.imageUrl} alt={category.name} className="w-10 h-10 object-cover rounded" />
        ) : (
             <span className="shrink-0 w-11 h-11 rounded-xl bg-[#fb6900]/10 text-[#fb6900] flex items-center justify-center transition-transform duration-200 group-hover:scale-105">
              <ChartBarStacked size={20} />
             </span>
        )}
      </span>
      <span className="text-sm font-medium text-gray-900">{category.name}</span>
      <span className="text-xs text-gray-400">1 món</span>
    </button>
  );
}

// ─── CategoryList ─────────────────────────────────────────────────
export function CategoryList({ categories }: CategoryListProps) {
  const [activeId, setActiveId] = useState<string | null>(
    categories.length ? categories[0].id : null
  );

  if (!categories.length) {
    return null; // hoặc render empty-state tuỳ ý
  }

  return (
    <section className="px-8 py-10 max-w-7xl mx-auto">
      {/* Section header */}
      <div className="flex items-baseline justify-between mb-7">
        <h2 className="font-display text-2xl font-bold tracking-tight">Danh mục món</h2>
        <a href="/list-category" className="text-sm font-medium text-brand hover:underline">
          Xem tất cả →
        </a>
      </div>

      {/* Category grid */}
      <div className="grid grid-cols-5 gap-3">
        {categories.map((cat) => (
          <CategoryItem
            key={cat.id}
            category={cat}
            isActive={activeId === cat.id}
            onClick={setActiveId}
          />
        ))}
      </div>
    </section>
  );
}