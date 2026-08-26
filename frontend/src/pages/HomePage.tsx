import { useState, useCallback} from 'react';
import { useQuery } from '@tanstack/react-query';
import { HeaderNav }    from '../components/layout/HeaderNav';
import { Footer }       from '../components/layout/Footer';
import { HeroSection }  from '../components/sections/HeroSection';
import { CategoryList } from '../components/sections/CategoryList';
import { DishGrid }     from '../components/sections/DishCard';
import { PromoSection } from '../components/sections/PromoSection';
import Skeleton from '../components/ui/Skeleton';
import { categoryService } from '../services/category.service';
import { Category } from '@/types/category';

export function HomePage() {
  const [cartCount, setCartCount] = useState(0);

  const {
    data: categories,
    isLoading: isCategoriesLoading,
  } = useQuery({
    queryKey: ['categories'],
    queryFn: async (): Promise<Category[]> => {
      const res = await categoryService.getAll({ page: 1, pageSize: 10 });
      return res.data.data.map((c) => ({
        id: c.id,
        name: c.name,
        description: c.description,
        displayOrder: c.displayOrder,
        imageUrl: c.imageUrl,
      }));
    },
    staleTime: 5 * 60 * 1000, // 5 phút — danh mục ít khi đổi
  });

  const handleAddToCart = useCallback(() => {
    setCartCount((prev) => prev + 1);
    // TODO: thay bằng context/zustand khi có CartContext
    console.log('Thêm vào giỏ:');
  }, []);

  return (
    <div className="min-h-screen bg-surface font-sans text-slate-900">
      <HeaderNav cartCount={cartCount} />

      <main className="pt-6">
        <HeroSection />

        <section className="mx-auto max-w-7xl space-y-10 px-4 py-10 md:px-8">
          {isCategoriesLoading ? (
            <CategoryListSkeleton />
          ) : (
            <CategoryList categories={categories ?? []} />
          )}
          <DishGrid onAddToCart={handleAddToCart} />
          <PromoSection />
        </section>
      </main>

      <Footer />
    </div>
  );
}

function CategoryListSkeleton() {
  return (
    <section className="px-8 py-10 max-w-7xl mx-auto">
      <div className="flex items-baseline justify-between mb-7">
        <Skeleton shape="text" width={180} height={28} />
        <Skeleton shape="text" width={80} height={18} />
      </div>
      <div className="grid grid-cols-5 gap-3">
        {Array.from({ length: 5 }).map((_, i) => (
          <Skeleton key={i} shape="rect" height={120} className="rounded-2xl" />
        ))}
      </div>
    </section>
  );
}