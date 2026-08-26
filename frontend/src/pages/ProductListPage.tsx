import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { Search, SlidersHorizontal, Utensils, ChevronDown, RefreshCw } from 'lucide-react';

import { Footer } from "@/components/layout/Footer";
import { HeaderNav } from "@/components/layout/HeaderNav";

import { productService } from '@/services/product.service.ts';
import { Dish } from '@/types';
import Skeleton from '@/components/ui/Skeleton';
import { DishCard } from '@/components/sections/DishCard';
import { useSearchParams } from 'react-router-dom';

// ─── MAIN PRODUCT LIST PAGE ───────────────────────────────────────
export const ProductListPage = () => {
  const [cartCount, setCartCount] = useState(0);
  const [searchTerm, setSearchTerm] = useState('');
  const [sortBy, setSortBy] = useState<string>('popular');
  const [page, _setPage] = useState(1);
  const pageSize = 10;
  const [searchParams] = useSearchParams();
  const categoryId = searchParams.get('categoryId');

  // React Query Fetch Data
  const { data: productsData, isLoading , error, refetch  } = useQuery({
    queryKey: ['products', { categoryId, page, pageSize }],
    queryFn: async () => {
      const res = await productService.getAll({ 
        page: page, 
        pageSize: pageSize,
        categoryId: categoryId || undefined 
      });
      return res.data;
    },
  });

  const handleAddToCart = (_dish: Dish) => {
    setCartCount((prev) => prev + 1);
    // Tích hợp logic lưu vào context / Redux / Zustand tại đây nếu cần
  };

  return (
    <div className="min-h-screen bg-surface font-sans text-slate-900 flex flex-col justify-between">
      {/* Header */}
      <HeaderNav cartCount={cartCount} />

      <main className="pt-6 pb-16 flex-1">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          
          {/* Banner & Filter Bar */}
          <div className="mb-8 space-y-6">
            <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
              <div>
                <h1 className="text-3xl font-bold tracking-tight text-slate-900 font-display">
                  Khám Phá Món Ăn
                </h1>
                <p className="text-sm text-slate-500 mt-1">
                  Thưởng thức hàng ngàn món ăn ngon giao tận nơi
                </p>
              </div>

              {/* Search Box */}
              <div className="relative w-full md:w-80">
                <Search className="absolute left-3.5 top-1/2 -translate-y-1/2 text-slate-400" size={18} />
                <input
                  type="text"
                  placeholder="Tìm kiếm món ăn, nhà hàng..."
                  value={searchTerm}
                  onChange={(e) => setSearchTerm(e.target.value)}
                  className="w-full pl-10 pr-4 py-2.5 bg-white border border-slate-200 rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-brand/20 focus:border-brand transition-all shadow-sm"
                />
              </div>
            </div>

            {/* Filter Pills & Sorting */}
            <div className="flex flex-wrap items-center justify-between gap-4 pt-4 border-t border-slate-200/60">
              {/* <div className="flex items-center gap-2 overflow-x-auto pb-2 md:pb-0 scrollbar-none">
                <button
                  onClick={() => setSelectedCategory('all')}
                  className={`px-4 py-2 rounded-xl text-sm font-medium transition-all whitespace-nowrap ${
                    selectedCategory === 'all'
                      ? 'bg-brand text-white shadow-md shadow-brand/20'
                      : 'bg-white text-slate-600 hover:bg-slate-100 border border-slate-200'
                  }`}
                >
                  Tất cả
                </button>
                {['Tất cả món', 'Cơm & Món mặn', 'Bún / Phở', 'Đồ ăn nhanh', 'Đồ uống / Trà sữa', 'Tráng miệng'].map((cat, idx) => (
                  <button
                    key={idx}
                    onClick={() => setSelectedCategory(cat)}
                    className={`px-4 py-2 rounded-xl text-sm font-medium transition-all whitespace-nowrap ${
                      selectedCategory === cat
                        ? 'bg-brand text-white shadow-md shadow-brand/20'
                        : 'bg-white text-slate-600 hover:bg-slate-100 border border-slate-200'
                    }`}
                  >
                    {cat}
                  </button>
                ))}
              </div> */}

              {/* Sort Dropdown */}
              <div className="flex items-center gap-2 self-end md:self-auto">
                <SlidersHorizontal size={16} className="text-slate-500" />
                <span className="text-xs font-medium text-slate-500 uppercase tracking-wider">Sắp xếp:</span>
                <div className="relative">
                  <select
                    value={sortBy}
                    onChange={(e) => setSortBy(e.target.value)}
                    className="appearance-none bg-white border border-slate-200 text-slate-700 py-2 pl-3 pr-8 rounded-xl text-sm font-medium focus:outline-none focus:ring-2 focus:ring-brand/20 cursor-pointer shadow-sm"
                  >
                    <option value="popular">Nổi bật nhất</option>
                    <option value="price-asc">Giá: Thấp đến Cao</option>
                    <option value="price-desc">Giá: Cao đến Thấp</option>
                    <option value="rating">Đánh giá cao</option>
                  </select>
                  <ChevronDown size={14} className="absolute right-2.5 top-1/2 -translate-y-1/2 text-slate-400 pointer-events-none" />
                </div>
              </div>
            </div>
          </div>

          {/* Grid Content */}
          {isLoading ? (
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
              {Array.from({ length: pageSize }).map((_, i) => (
                <div key={i} className="bg-white p-3 rounded-2xl border border-slate-100 space-y-3">
                  <Skeleton shape="rect" height={176} className="rounded-xl w-full" />
                  <Skeleton shape="rect" height={20} className="w-3/4 rounded-md" />
                  <Skeleton shape="rect" height={14} className="w-1/2 rounded-md" />
                  <div className="flex justify-between items-center pt-2">
                    <Skeleton shape="rect" height={24} className="w-1/3 rounded-md" />
                    <Skeleton shape="circle" height={32} className="w-8" />
                  </div>
                </div>
              ))}
            </div>
          ) : error ? (
            <div className="text-center py-16 bg-white rounded-2xl border border-slate-200 shadow-sm">
              <p className="text-slate-600 font-medium mb-4">Không thể tải danh sách sản phẩm lúc này.</p>
              <button 
                onClick={() => refetch()}
                className="inline-flex items-center gap-2 px-4 py-2 bg-brand text-white rounded-xl text-sm font-medium hover:bg-brand-dark transition-colors"
              >
                <RefreshCw size={16} /> Thử lại
              </button>
            </div>
          ) : productsData?.data.length === 0 ? (
            <div className="text-center py-16 bg-white rounded-2xl border border-slate-200 shadow-sm">
              <Utensils className="mx-auto text-slate-300 mb-3" size={48} />
              <h3 className="text-base font-semibold text-slate-800">Không tìm thấy món ăn phù hợp</h3>
              <p className="text-slate-500 text-sm mt-1">Vui lòng thử tìm kiếm lại với từ khóa hoặc bộ lọc khác.</p>
            </div>
          ) : (
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
              {productsData?.data.map((dish) => (
                <DishCard key={dish.id} dish={dish} onAddToCart={handleAddToCart} />
              ))}
            </div>
          )}

        </div>
      </main>

      {/* Footer */}
      <Footer />
    </div>
  );
};