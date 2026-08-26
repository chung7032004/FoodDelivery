import { Link } from 'react-router-dom';
import { ChartBarStacked } from 'lucide-react';
import { HeaderNav } from '@/components/layout/HeaderNav';
import { Footer } from '@/components/layout/Footer';
import { useState } from 'react';
import { Category } from '@/types/category';
import { useQuery } from '@tanstack/react-query';
import { categoryService } from '@/services/category.service';

export function ListCategoryPage() {

  // const categories = [...CATEGORIES].sort((a, b) => a.displayOrder - b.displayOrder);
  const [cartCount, setCartCount] = useState(0);

  const {data : categories ,isLoading : isCategoriesLoading} = useQuery({
    queryKey: ['categories'],
    queryFn: async () :Promise<Category[]> => {
      const res = await categoryService.getAll({ page: 1, pageSize: 10 });
      return res.data.data.map((c) => ({
        id: c.id,
        name: c.name,
        description: c.description,
        displayOrder: c.displayOrder,
        imageUrl: c.imageUrl,
      }));
    },
    staleTime : 5 * 60 * 1000, // 5 phút — danh mục ít khi đổi
  }); 

  return (
    <div className="min-h-screen bg-surface font-sans text-slate-900">
      <HeaderNav cartCount={cartCount} />
      <section className="max-w-7xl mx-auto px-6 lg:px-8 py-10">
        <h1 className="font-display text-2xl font-bold text-black">
          Thực đơn theo <span className="text-orange-500">danh mục</span>
        </h1>
        <p className="mt-1 text-sm text-muted">Chọn một danh mục để xem món ăn</p>
        <div className="mt-6 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
          {(categories || []).map((cat) => {
            return (
              <Link
                key={cat.id} 
                to={`/products?categoryId=${cat.id}`}
                className="group flex items-start gap-3.5 rounded-2xl border border-border bg-surface p-4 transition-all duration-200 hover:border-[#fb6900]/30 hover:bg-[#fb6900]/5"
              >
                <span className="shrink-0 w-11 h-11 rounded-xl bg-[#fb6900]/10 text-[#fb6900] flex items-center justify-center transition-transform duration-200 group-hover:scale-105">
                  {
                    cat.imageUrl ? <img src={cat.imageUrl} alt={cat.name} className="w-6 h-6 object-cover rounded " />
                    : <ChartBarStacked size={20} />
                  }
                </span>
                <div className="min-w-0">
                  <h2 className="font-semibold text-sm text-black group-hover:text-orange-500 transition-colors">
                    {cat.name}
                  </h2>
                  <p className="mt-0.5 text-xs text-muted leading-snug line-clamp-2">
                    {cat.description}
                  </p>
                </div>
              </Link>
            );
          })}
        </div>
      </section>
      <Footer />
    </div>
  );
}